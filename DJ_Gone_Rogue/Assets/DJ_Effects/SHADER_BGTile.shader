Shader "Custom/SHADER_BGTile" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_YOffset ("Height offset", float) = 0.0
		_Scale ("Scale", Vector) = (1.0,1.0,1.0,1.0)
	}
	SubShader {
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _YOffset;
			uniform fixed4 _Scale;
			
			uniform float4 _MainTex_ST;
			
			struct vertInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};
			
		    v2f_img vert (vertInput v)
       	    {
	        	v2f_img o;
	        	
	        	v.vertex.y += _YOffset;
	        	
		        o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		        
		        o.pos.x *= _Scale.x;
		        o.pos.y *= _Scale.y;
		        o.pos.z *= _Scale.z;
		        
		        o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		        
		        return o;
		    }
			
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
