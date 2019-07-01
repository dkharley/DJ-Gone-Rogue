Shader "DJ/Effects/Shaders/DeathEffectShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ScreenWidth( "Width of screen", int) = 1024
		_ScreenHeight( "Width of screen", int) = 1024
		_Duration("Duration of effect", float) = 1.0
		_CurrTime("Current effect time", float) = 1.0
		_NumBars("Number of bars", int) = 20.0
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

			uniform sampler2D _MainTex;
			uniform int _ScreenWidth;
			uniform int _ScreenHeight;
			uniform float _Duration;
			uniform float _CurrTime;
			uniform int _NumBars;
			
			fixed4 frag (v2f_img i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv);
				
				if(_CurrTime <= _Duration)
				{
					half _lineWeight = .1;
					
					half _aspect = _ScreenWidth / _ScreenHeight;
					
					half _barWidth = 1.0 / _NumBars;
					half _barHeight = 1.0 / _NumBars * _aspect;
					
					half _radius = 1.0 - _CurrTime / _Duration * 2.0;
					
					half2 _center = half2(.5, .5);
					
					half2 _v = normalize(i.uv - _center) * _lineWeight / 2.0;
					
					half2 _testPoint = i.uv + _v;
					
					half _dist = distance( _testPoint, _center);
					
					int _barIndexX = floor(i.uv.x / _barWidth);
					int _barIndexY = floor(i.uv.y / _barHeight);
					
					if( _dist > _radius && !(_dist > _radius - .1 && _dist < _radius + .1))
					{
						half _alpha = 1.0 - abs(_radius - _dist) / _lineWeight;
						_alpha = _alpha * _alpha * _alpha * _alpha * _alpha * _alpha;
						
						fixed4 _one = fixed4(0.0,0.0,0.0,1.0);
						fixed4 _two = fixed4(0.0,0.0,0.0,1.0);
						
						fixed _val1 = abs(.1 * (1.0 - _radius - _CurrTime / _Duration * _CurrTime / _Duration)) * _CurrTime / _Duration;
						fixed _val2 = abs(.2 * (1.0 - _radius - _CurrTime / _Duration * _CurrTime / _Duration));
						
						if(_barIndexX % 2 == 0)
							_one = fixed4(_val2,_val2,_val2,1.0);
						else if(_barIndexX % 3 == 0)
							_one = fixed4(_val2 * 1.5,_val2 * 1.5,_val2 * 1.5,1.0);
						else
							_one = fixed4(_val1,_val1,_val1,1.0);
							
						if(_barIndexY % 2 == 0)
							_two = fixed4(_val2,_val2,_val2,1.0);
						else if(_barIndexX % 3 == 0)
							_two = fixed4(_val2 * 1.5,_val2 * 1.5,_val2 * 1.5,1.0);
						else
							_two = fixed4(_val1,_val1,_val1,1.0);
						
						color = (color - (_one + _two) * _alpha);
					}
				}
				else
				{
					color = color * (_CurrTime * .5 / (_Duration)) * (_CurrTime * .5 / (_Duration));
					color = color * (_CurrTime * .5 / (_Duration)) * (_CurrTime * .5 / (_Duration));
					color = color * (_CurrTime * .5 / (_Duration)) * (_CurrTime * .5 / (_Duration));
				}
				
//				if(_dist > _radius - .01 && _dist < _radius + .01)
//					color = fixed4(1.0,0.0,0.0,1.0);
				
				color.a = 1.0;
				
				return color;
			}
			
//				//////////////////////////////////////
//				///START EFFECT #1
//				///////////////////////////////
//				
//				half _startY = 1.25;
//				half _endY = -.25;
//				half _currY = ( (_endY - _startY) / _Duration) * _CurrTime + _startY;
//				
//				half2 _point = half2(.5, _currY);
//				
//				//calc the width of each  bar
//				half _barWidth = 1.0 / _NumBars;
//				
//				//get the bar index
//				int _barIndex = floor(i.uv.x / _barWidth);
//				
//				int _heightIndex = _barIndex % 10;
//			
//				half _height0 = .5;
//				half _height1 = .85;
//				half _height2 = 1.15;
//				half _height3 = .9;
//				half _height4 = .75;
//				half _height5 = 1.43;
//				half _height6 = .81;
//				half _height7 = .98;
//				half _height8 = 1.32;
//				half _height9 = 1.0;
//				
//				half _offset0 = -.1;
//				half _offset1 = .2;
//				half _offset2 = -.45;
//				half _offset3 = .3;
//				half _offset4 = -.0;
//				half _offset5 = -.5;
//				half _offset6 = .25;
//				half _offset7 = .1;
//				half _offset8 = -.4;
//				half _offset9 = .25;
//				
//				half _barHeight = _height0;
//				half _barOffset = _offset0;
//				
//				//find the appropriate bar height
//				if(_heightIndex == 0)
//				{
//					_barHeight = _height0 * 2;
//					_barOffset = _offset0;
//				}
//				else if(_heightIndex == 1)
//				{
//					_barHeight = _height1 * 2;
//					_barOffset = _offset1;
//				}
//				else if(_heightIndex == 2)
//				{
//					_barHeight = _height2 * 2;
//					_barOffset = _offset2;
//				}
//				else if(_heightIndex == 3)
//				{
//					_barHeight = _height3 * 2;
//					_barOffset = _offset3;
//				}
//				else if(_heightIndex == 4)
//				{
//					_barHeight = _height4 * 2;
//					_barOffset = _offset4;
//				}
//				else if(_heightIndex == 5)
//				{
//					_barHeight = _height5 * 2;
//					_barOffset = _offset5;
//				}
//				else if(_heightIndex == 6)
//				{
//					_barHeight = _height6 * 2;
//					_barOffset = _offset6;
//				}
//				else if(_heightIndex == 7)
//				{
//					_barHeight = _height7 * 2;
//					_barOffset = _offset7;
//				}
//				else if(_heightIndex == 8)
//				{
//					_barHeight = _height8 * 2;
//					_barOffset = _offset8;
//				}
//				else if(_heightIndex == 9)
//				{
//					_barHeight = _height9 * 2;
//					_barOffset = _offset9;
//				}
//				float _dy = (_barIndex * _CurrTime / _Duration) / 3.0;
//				
//				fixed _yCoord = i.uv.y;
//				
//				half _xOffset = _barWidth * _barIndex;
//				
//				half _newX = i.uv.x - _xOffset;
//				
//				if(_newX < 0.0)
//					_newX = 1.0 - _newX;
//				
//				half _newY = _yCoord + _barOffset + _dy;
//				
//				if(_newY > 1.0)
//					_newY = _newY - 1.0;
//				
//				//find the offset color in case we are coloring a bar
//				fixed4 _temp = tex2D(_MainTex, half2(_newX, _newY));
//				
//				if( abs((_yCoord + _barOffset + _dy) - _currY) < _barHeight / 2.0)
//				{
//					//set the color of the bar
//					if(_barIndex % 2 == 0)
//						color = fixed4(0.0,0.0,0.0,1.0);
//					else
//						color = fixed4(1.0,1.0,1.0,1.0);
//					//color = _temp;
//				}
//				
//				//uncomment to make screen into barcode
////				if(_barIndex % 2 == 0)
////					color = fixed4(0.0,0.0,0.0,1.0);
//				
////				if( distance(i.uv, _point) < .01)
////					color = fixed4(1.0,0.0,0.0,1.0);
//				/////////////////////////////////////////////////
//				// END EFFECT #1
//				//////////////////////////////////////////////
			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
