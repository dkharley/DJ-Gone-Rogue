Shader "Glow 11/Unity/Transparent/Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _GlowTex ("Glow", 2D) = "" {}
    _GlowColor ("Glow Color", Color)  = (1,1,1,1)
    _GlowStrength ("Glow Strength", Float) = 1.0
    _Alpha ("Alpha", Float) = 1.0
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderEffect"="Glow11Transparent" "RenderType"="Glow11Transparent" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert alpha

sampler2D _MainTex;
fixed4 _Color;
uniform float _Alpha;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	_Alpha = clamp(_Alpha, 0.0, 1.0);
	o.Albedo = c.rgb;
	
	o.Emission = c.rgb * UNITY_SAMPLE_1CHANNEL(_MainTex, IN.uv_MainTex) * _Alpha;
	o.Alpha = c.a * _Alpha;
}
ENDCG
}

FallBack "Glow 11/Unity/Transparent/VertexLit"
CustomEditor "GlowMatInspector"
}
