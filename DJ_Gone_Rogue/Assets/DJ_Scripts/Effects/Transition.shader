Shader "DJ/Effects/Shaders/Transition" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EffectDuration ("Duration of effect", float) = 2
		_CurrTime ("Current Time (Float)", Float) = 0
		_ScreenWidth( "Width of screen", int) = 1024
		_ScreenHeight( "Width of screen", int) = 1024
		_WidthOfLine ("Width of line", float) = 6
		_R ("R Channel Ratio", float) = 1
		_G ("G Channel Ratio", float) = 1
		_B ("B Channel Ratio", float) = 1
	}
	SubShader {
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			const float PI = 3.14159265359;
			const float resolution = 260;
			
			uniform sampler2D _MainTex;
			uniform float _CurrTime;
			uniform float _EffectDuration;
			uniform float _WidthOfLine;
			uniform int _ScreenWidth;
			uniform int _ScreenHeight;
			uniform float _R;
			uniform float _G;
			uniform float _B;
			
			fixed hash(fixed n)
			{
				return frac(sin(n) * 43758.5453);
			}
			
			fixed noise(fixed3 x)
			{
				fixed3 p = floor(x);
				fixed3 f = frac(x);
				
				f = f*f*(3.0 - 2.0*f);
				fixed n = p.x + p.y*57.0 + 113.0*p.z;
				
				return lerp(lerp(lerp(hash(n+0.0), hash(n+1.0),f.x),
					   lerp(hash(n+57.0),hash(n+58.0),f.x),f.y),
					   lerp(lerp(hash(n+113.0),hash(n+114.0),f.x),
					   lerp(hash(n+170.0),hash(n+171.0),f.x),f.y),f.z);
			}
			
			fixed map(fixed value, fixed from1, fixed to1, fixed from2, fixed to2)
			{
				return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
			}
			
			fixed getAngle(fixed x, fixed y)
			{
				fixed angle = atan2(x, y);
				
				return angle;
			}
			
			
			fixed getRadiusOffset(fixed2 point)
			{
				fixed2 center = fixed2(.5, .5);
				fixed _offset = 0.0;
				fixed2 _v = point - center;
				fixed _a = atan(_v);
				
				fixed _x = map(_a, 0, 2 * PI, 0.0, 1.0);
				
				fixed _amp = cos(_x) * .1;
				
				_offset = _amp;
				
				return _offset;
			}
			
			fixed4 getBandColor(fixed4 color, fixed radius, fixed dist, fixed lineWeight)
			{
					half alpha = 1.0 - abs(radius - dist) / lineWeight;
					alpha = alpha * alpha * alpha * alpha * alpha * alpha;
					alpha = alpha * alpha * alpha;
					color = (color + fixed4(1.0,1.0,1.0, 1.0) * alpha);
					return color;
			}
			
			fixed4 getBandColor(fixed4 color, fixed2 p1, fixed2 p2, fixed lineWeight)
			{
					fixed dist = distance(p1,p2);
					half alpha = 1.0 - abs(dist) / lineWeight;
					alpha = alpha * alpha * alpha * alpha * alpha * alpha;
					alpha = alpha * alpha * alpha;
					color = (color + fixed4(.0,1.0,1.0, 1.0) * alpha);
					return color;
			}
			
			fixed4 determineGrayScale(fixed4 color, fixed dist, fixed radius, fixed lineWeight)
			{
				//apply grayscale
				if(dist > radius)
				{
					//color = color * _CurrTime;
					float avg = (color.r + color.g + color.b) / 3;
					
					color.r = avg;
					color.g = avg;
					color.b = avg;
				}
				//apply prev color ramp
				if(dist <= radius + lineWeight / 2.0 && dist >= radius - lineWeight / 2.0)
				{
					color = getBandColor(color, radius, dist, lineWeight);
				}
				
				return color;
			}
			
			fixed4 getRampedColor(v2f_img i)
			{
				fixed4 orig = tex2D(_MainTex, i.uv);
				
				fixed4 ramping = fixed4(_R, _G, _B, 1.0);
				
				fixed4 color = orig * ramping;
				
				return color;
			}
			
			//effect duration = perlin noise input. slide this value up and down
			//lineWeight 334
			//currtime = any
			fixed4 nicePerlinBG1(v2f_img i)
			{
				fixed4 color = getRampedColor(i);
				
				fixed lineWeight = _WidthOfLine / 100.0;
				
				fixed _noise = noise(fixed3(i.uv.x, i.uv.y, _CurrTime));
				
				fixed2 p = fixed2(i.uv.x, sin( map(_noise, 0.0, 10.0, 0.0, 360.0 ) ) );
				
				if(distance(p, i.uv) < lineWeight)
					color = getBandColor(color, i.uv, p, lineWeight);
				
				return color;
			}
			
			fixed4 nicePerlinBG2(v2f_img i)
			{
				fixed4 color = getRampedColor(i);
				
				fixed lineWeight = _WidthOfLine / 100.0;
				
				fixed _noise1 = noise(fixed3(i.uv.x, i.uv.y, _CurrTime));
				fixed _noise2 = noise(fixed3(i.uv.y, i.uv.x, _CurrTime));
				
				fixed2 p = fixed2(sin( map(_noise2 * _noise2, 0.0, 10.0, 0.0, 360.0 ) ), sin( map(_noise1 * _noise1, 0.0, 10.0, 0.0, 360.0 ) ) );
				
				if(distance(p, i.uv) < lineWeight)
					color = getBandColor(color, i.uv, p, lineWeight);
				
				return color;
			}
			
			fixed4 test(v2f_img i)
			{
				fixed4 color = getRampedColor(i);
				
				fixed lineWeight = _WidthOfLine / 100.0;
				
				fixed _rBasis = (1.0 - _CurrTime / _EffectDuration);
				
				fixed _targetRadius = distance(i.uv, fixed2(.5, .5));
				
				color = determineGrayScale(color, _targetRadius, _rBasis, lineWeight);
				
				return color;
			}
			
			fixed4 frag (v2f_img i) : COLOR
			{
				fixed4 color = test(i);
				
				return color;
			}
			
			/////////////////////////////////////////////////////////
			//OLD CODE OLD CODE OLD CODE OLD CODE OLD CODE OLD CODE//
			/////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////
			//OLD CODE OLD CODE OLD CODE OLD CODE OLD CODE OLD CODE//
			/////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////
			//OLD CODE OLD CODE OLD CODE OLD CODE OLD CODE OLD CODE//
			/////////////////////////////////////////////////////////
			/////////////////////////////////////////////////////////
			//OLD CODE OLD CODE OLD CODE OLD CODE OLD CODE OLD CODE//
			/////////////////////////////////////////////////////////
			
			
//			//pseudo-noise function
//			fixed rand(fixed x, fixed y, fixed z)
//			{
//				fixed3 co = fixed3(x,y, z);
//			    return frac(sin(dot(co.xyz ,fixed3(12.9898,78.233, 22.31355))) * 43758.5453);
//			}
//			
//			fixed rand(fixed x, fixed y)
//			{
//				fixed2 co = fixed2(x,y);
//			    return frac(sin(dot(co.xy ,fixed2(12.9898,78.233))) * 43758.5453);
//			}
//			
//			fixed perlinCircleRadius1(v2f_img  i)
//			{
//				fixed radius = 1.0 - _CurrTime / _EffectDuration;
//				fixed nInt = map(i.uv.x, 0.0, 1.0, .1, 30);
//				fixed nAmp = map(i.uv.y, 0.0, 1.0, 0.0, .1);
//				fixed2 center = fixed2(.5, .5);
//				fixed2 disp = i.uv - center;
//				
//				fixed angle = atan2(disp.x, disp.y);
//				
//				if(disp.x >= 0.0 && disp.y >= 0.0)
//				{
//					//angle = angle;
//				}
//				else if(disp.x < 0.0 && disp.y >= 0.0)
//				{
//					angle = PI - angle;
//				}
//				else if(disp.x < 0.0 && disp.y < 0.0)
//				{
//					angle = angle + PI;
//				}
//				else if(disp.x >= 0.0 && disp.y < 0.0)
//				{
//					angle = 2 * PI - angle;
//				}
//				
//				fixed nVal = map(rand(cos(angle) * nInt + 1.0, sin(angle) * nInt + 1, _CurrTime), 0.0, 1.0, nAmp, 1.0);
//				
//				fixed _x = cos(angle) * radius * nVal;
//				fixed _y = sin(angle) * radius * nVal;
//				
//				fixed _offset = sqrt(_x * _x + _y * _y);
//				
//				return _offset;
//			}
//			
//			fixed perlinCircleRadius2(v2f_img  i)
//			{
//				fixed radius = 1.0 - _CurrTime / _EffectDuration;
//				fixed nInt = map(i.uv.x, 0.0, 1.0, .1, 30);
//				fixed nAmp = map(i.uv.y, 0.0, 1.0, 0.0, .1);
//				fixed2 center = fixed2(.5, .5);
//				fixed2 disp = i.uv - center;
//				
//				fixed angle = atan2(disp.x, disp.y);
//				
//				if(disp.x >= 0.0 && disp.y >= 0.0)
//				{
//					//angle = angle;
//				}
//				else if(disp.x < 0.0 && disp.y >= 0.0)
//				{
//					angle = 180 - angle;
//				}
//				else if(disp.x < 0.0 && disp.y < 0.0)
//				{
//					angle = angle + 180;
//				}
//				else if(disp.x >= 0.0 && disp.y < 0.0)
//				{
//					angle = 360 - angle;
//				}
//				
//				angle = PI / 180 * angle;
//				
//				fixed nVal = map(rand(cos(angle) * nInt + 1.0, sin(angle) * nInt + 1, _CurrTime), 0.0, 1.0, nAmp, 1.0);
//				
//				fixed _x = cos(angle) * radius * nVal;
//				fixed _y = sin(angle) * radius * nVal;
//				
//				fixed _offset = sqrt(_x * _x + _y * _y);
//				
//				return _offset;
//			}
//			
//			fixed perlinCircleRadius3(v2f_img  i)
//			{
//				fixed radius = 1.0 - _CurrTime / _EffectDuration;
//				fixed nInt = map(i.uv.x, 0.0, 1.0, .1, 30);
//				fixed nAmp = map(i.uv.y, 0.0, 1.0, 0.0, .1);
//				fixed2 center = fixed2(.5, .5);
//				fixed2 disp = i.uv - center;
//				
//				fixed angle = atan2(disp.x, disp.y);
//				
//				if(disp.x >= 0.0 && disp.y >= 0.0)
//				{
//					//angle = angle;
//				}
//				else if(disp.x < 0.0 && disp.y >= 0.0)
//				{
//					angle = 180 - angle;
//				}
//				else if(disp.x < 0.0 && disp.y < 0.0)
//				{
//					angle = angle + 180;
//				}
//				else if(disp.x >= 0.0 && disp.y < 0.0)
//				{
//					angle = 360 - angle;
//				}
//				
//				angle = PI / 180 * angle;
//				
//				fixed nVal = map(rand(cos(angle) * nInt + 1.0, sin(angle) * nInt + 1, _CurrTime), 0.0, 1.0, nAmp, 1.0);
//				
//				fixed _x = cos(angle) * radius * nVal;
//				fixed _y = sin(angle) * radius * nVal;
//				
//				fixed _offset = sqrt(_x * _x + _y * _y);
//				
//				return _offset;
//			}
//			
//			half noise(half seed)
//			{
//				half val = 0.0;
//				half amount = 1.0;
//				
//				if(fmod(seed, 2.0) < .001)
//					val = .1;
//				if(fmod(seed, 3.0) < .001)
//					val = .15;
//				if(fmod(seed, 4.0) < .001)
//					val = .05;
//				if(fmod(seed, 5.0) < .001)
//					val = .25;
//				if(fmod(seed, 6.0) < .001)
//					val = .2;
//				
//				if(sin(seed) < 0.0)
//					amount *= -1;
//				
//				return seed * amount;
//			}
//			
//			fixed4 effect1(v2f_img i)
//			{
//				fixed4 orig = tex2D(_MainTex, i.uv);
//				
//				fixed4 ramping = fixed4(_R, _G, _B, 1.0);
//				
//				fixed4 color = orig * ramping;
//				
//				fixed lineWeight = _WidthOfLine / 100.0;
//				
//				half2 center = half2(.5,.5);
//				
//				fixed dist = distance(i.uv, center);
//				
//				fixed radius = 1.0 - _CurrTime / _EffectDuration;
//				
//				//apply next color ramp
//				if(dist > radius)
//				{
//					//color = color * _CurrTime;
//					float avg = (color.r + color.g + color.b) / 3;
//					
//					color.r = avg;
//					color.g = avg;
//					color.b = avg;
//				}
////				else if(dist <= radius + lineWeight / 50.0 && dist >= radius - lineWeight / 50.0)
////				{
////					color = fixed4(1.0,0.0,0.0,1.0);
////				}
//				//apply prev color ramp
//				if(dist <= radius + lineWeight / 2.0 && dist >= radius - lineWeight / 2.0)
//				{
//					color = getBandColor(color, radius, dist, lineWeight);
//				}
////
//				return color;
//			}
//			
//			fixed4 effect2(v2f_img i)
//			{
//				fixed4 orig = tex2D(_MainTex, i.uv);
//				
//				fixed4 ramping = fixed4(_R, _G, _B, 1.0);
//				
//				fixed4 color = orig * ramping;
//				
//				fixed lineWeight = _WidthOfLine / 100.0;
//				
////				fixed currX = _CurrTime / _EffectDuration;
////				
////				fixed minX = currX - lineWeight / 2.0;
////				fixed maxX = currX + lineWeight / 2.0;
//				
//				half2 center = half2(.5,.5);
//				
//				half _x = i.uv.x - center.x;
//				half _y = i.uv.y - center.y;
//				
//				half angle = atan2(_x, _y);
//				
//				half _offset = noise(angle);
//				
//				//twerkin booty
////				fixed dist = distance(i.uv, center) - sin(cos(1.0 / noise(_offset * _offset) + _offset * noise(_CurrTime / _EffectDuration)));
//				
//				fixed dist = distance(i.uv, center);
//				
//				
//				//fixed radius = 1.0 - _CurrTime / _EffectDuration;
////				fixed radius =  dist + sin(cos(1.0 / noise(_offset * _offset) + _offset * noise(_CurrTime / _EffectDuration)));
////				cool top effect. beams of light
//				fixed radius =  1.0 - _CurrTime / _EffectDuration + sin(cos(1.0 / noise(_offset * _offset) + _offset * noise(_CurrTime / _EffectDuration)));
//
//				//circle stuff
////				fixed _temp = 1.0 / noise(_offset * _offset) + _offset * noise(_CurrTime / _EffectDuration);
////				
////				fixed radius =  sin(lerp(cos(_temp), _temp, noise(_temp)));
//				
//				//fixed radius = 1.0 - _CurrTime / _EffectDuration;
//				
//				//apply next color ramp
//				if(dist > radius)
//				{
//					//color = color * _CurrTime;
//					float avg = (color.r + color.g + color.b) / 3;
//					
//					color.r = avg;
//					color.g = avg;
//					color.b = avg;
//				}
////				else if(dist <= radius + lineWeight / 50.0 && dist >= radius - lineWeight / 50.0)
////				{
////					color = fixed4(1.0,0.0,0.0,1.0);
////				}
//				//apply prev color ramp
//				if(dist <= radius + lineWeight / 2.0 && dist >= radius - lineWeight / 2.0)
//				{
//					color = getBandColor(color, radius, dist, lineWeight);
//				}
////
//				return color;
//			}
//			
//			//weird fun house mirror
//			fixed4 effect3(v2f_img i)
//			{
//				fixed4 color = tex2D(_MainTex, i.uv);
//				
//				fixed p = -1.0 + 2.0 * i.uv;
//				
//				fixed len = length(p);
//				
//				fixed2 uv = i.uv + (p / len) * cos(len * 12.0 - _CurrTime * 4.0) * 0.03 * _CurrTime / _EffectDuration;
//				
//				fixed3 col = tex2D(_MainTex, uv).xyz;
//				
//				color = fixed4(col, 1.0);
//				
//				return color;
//			}
//			
//			//length of effect: 22.7
//			//CurrTime: 100.58
//			// line weight: 20
//			fixed4 effect4(v2f_img i)
//			{
//				fixed4 color = tex2D(_MainTex, i.uv);
//				
//				fixed p = -1.0 + 5.0 * i.uv;
//				
//				fixed len = length(p);
//				
//				fixed2 uv = i.uv - (p / len) * cos(len * 12.0 - _CurrTime * 4.0) * 0.03 * _CurrTime / _EffectDuration;
//				
//				fixed3 col = tex2D(_MainTex, uv).xyz;
//				
//				color = fixed4(col, 1.0);
//				
//				return color;
//			}
//			
//			fixed4 perlinCirleEffect1(v2f_img i)
//			{
//				fixed4 orig = tex2D(_MainTex, i.uv);
//				
//				fixed4 ramping = fixed4(_R, _G, _B, 1.0);
//				
//				fixed4 color = orig * ramping;
//				
//				fixed lineWeight = _WidthOfLine / 100.0;
//				
//				half2 center = half2(.5,.5);
//				
//				fixed dist = distance(i.uv, center);
//				
//				fixed radius = perlinCircleRadius1(i);
//				
//				//apply next color ramp
//				if(dist > radius)
//				{
//					//color = color * _CurrTime;
//					float avg = (color.r + color.g + color.b) / 3;
//					
//					color.r = avg;
//					color.g = avg;
//					color.b = avg;
//				}
////				else if(dist <= radius + lineWeight / 50.0 && dist >= radius - lineWeight / 50.0)
////				{
////					color = fixed4(1.0,0.0,0.0,1.0);
////				}
//				//apply prev color ramp
//				if(dist <= radius + lineWeight / 2.0 && dist >= radius - lineWeight / 2.0)
//				{
//					color = getBandColor(color, radius, dist, lineWeight);
//				}
////
//				return color;
//			}
//			
//			
//			//length of effect 24.4
//			//line weight 1.1
//			//curr time 19.2
//			fixed4 finalAttemptMaybe(v2f_img i)
//			{
//				fixed4 orig = tex2D(_MainTex, i.uv);
//				
//				fixed4 ramping = fixed4(_R, _G, _B, 1.0);
//				
//				fixed4 color = orig * ramping;
//				
//				fixed lineWeight = _WidthOfLine / 100.0;
//				
//				half2 center = half2(.5,.5);
//				
//				fixed radius = 1.0 - _CurrTime / _EffectDuration;
//				
//				fixed _offset = 0.0;
//				
//				fixed dist = distance(i.uv, center);
//				
//				radius = perlinCircleRadius1(i);
//				
//				//apply grayscale
//				if(dist > radius)
//				{
//					//color = color * _CurrTime;
//					float avg = (color.r + color.g + color.b) / 3;
//					
//					color.r = avg;
//					color.g = avg;
//					color.b = avg;
//				}
//				//apply prev color ramp
//				if(dist <= radius + lineWeight / 2.0 && dist >= radius - lineWeight / 2.0)
//				{
//					color = getBandColor(color, radius, dist, lineWeight);
//				}
////
//				return color;
//			}
//			
//			//effect duration 24.4
//			//lineWeight 68.4
//			//currtime 19.1
//			fixed4 final2(v2f_img i)
//			{
//				fixed4 orig = tex2D(_MainTex, i.uv);
//				
//				fixed4 ramping = fixed4(_R, _G, _B, 1.0);
//				
//				fixed4 color = orig * ramping;
//				
//				fixed lineWeight = _WidthOfLine / 100.0;
//				
//				half2 center = half2(.5,.5);
//				
//				fixed radius = 1.0 - _CurrTime / _EffectDuration;
//				
//				fixed _offset = 0.0;
//				
//				fixed dist = distance(i.uv, center);
//				
//				radius = perlinCircleRadius3(i);
//				
//				//apply grayscale
//				if(dist > radius)
//				{
//					//color = color * _CurrTime;
//					float avg = (color.r + color.g + color.b) / 3;
//					
//					color.r = avg;
//					color.g = avg;
//					color.b = avg;
//				}
//				//apply prev color ramp
//				if(dist <= radius + lineWeight / 2.0 && dist >= radius - lineWeight / 2.0)
//				{
//					color = getBandColor(color, radius, dist, lineWeight);
//				}
////
//				return color;
//			}
			
			ENDCG
		}
	} 
	FallBack Off
}
