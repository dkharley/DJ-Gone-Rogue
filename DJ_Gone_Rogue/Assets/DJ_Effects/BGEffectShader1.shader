Shader "DJ/Effects/Shaders/BGEffectShader1" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ScreenWidth( "Width of screen", int) = 1024
		_ScreenHeight( "Width of screen", int) = 1024
		_FadeSpeed("Fade speed", float) = 1.0
		_Pos1("Position of the next effect", Color) = (0,0,0,0)
		_Color("Color of effect", Color) = (1,1,1,1)
		_Size("Size of effect", float) = 1.0
	}
	SubShader {
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform int _ScreenWidth;
			uniform int _ScreenHeight;
			uniform float _FadeSpeed;
			uniform fixed4 _Pos1;
			uniform float _Size;

			fixed4 frag (v2f_img i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv);
				
				return color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
