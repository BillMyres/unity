Shader "Custom/TerrainCombine" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_NormalHeight("NormalHeight", Range(0, 1)) = 0.5
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_SplatMap("_SplatMap", 2D) = "white" {}

		_Tex1("R", 2D) = "white" {}
		_Tex1a("R Normal", 2D) = "bump" {}

		_Tex2("G", 2D) = "white" {}
		_Tex2a("G Normal", 2D) = "bump" {}

		_Tex3("B", 2D) = "white" {}
		_Tex3a("B Normal", 2D) = "bump" {}
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 4.0
	struct Input {
		float2 uv_Tex1;
		float2 uv_Tex1a;

		float2 uv_Tex2;
		float2 uv_Tex2a;

		float2 uv_Tex3;
		float2 uv_Tex3a;

		float2 uv_SplatMap;
	};
	sampler2D _Tex1;
	sampler2D _Tex1a;

	sampler2D _Tex2;
	sampler2D _Tex2a;

	sampler2D _Tex3;
	sampler2D _Tex3a;

	sampler2D _SplatMap;

	float4 _Color;
	half _NormalHeight;
	half _Glossiness;
	half _Metallic;

	void surf(Input IN, inout SurfaceOutputStandard o) {
		half4 vNotBlack = half4(1,1,1,1) - (tex2D(_SplatMap, IN.uv_SplatMap).r) - (tex2D(_SplatMap, IN.uv_SplatMap).g) - (tex2D(_SplatMap, IN.uv_SplatMap).b);

		o.Albedo = _Color;
		o.Albedo += ((tex2D(_Tex1, IN.uv_Tex1).rgb)*(tex2D(_SplatMap, IN.uv_SplatMap).r));
		o.Albedo += ((tex2D(_Tex2, IN.uv_Tex2).rgb)*(tex2D(_SplatMap, IN.uv_SplatMap).g));
		o.Albedo += ((tex2D(_Tex3, IN.uv_Tex3).rgb)*(tex2D(_SplatMap, IN.uv_SplatMap).b));
		//fixed4 something = tex2D(_Tex1a, IN.uv_Tex1a).rgb*(tex2D(_SplatMap, IN.uv_SplatMap).r);
		//o.Normal += UnpackNormal(tex2D(_Tex1a, IN.uv_Tex1a));
		o.Normal += UnpackNormal((tex2D(_Tex1a, IN.uv_Tex1a)*(tex2D(_SplatMap, IN.uv_SplatMap).r)) * _NormalHeight);
		o.Normal += UnpackNormal((tex2D(_Tex2a, IN.uv_Tex2a)*(tex2D(_SplatMap, IN.uv_SplatMap).g)) * _NormalHeight);
		o.Normal += UnpackNormal((tex2D(_Tex3a, IN.uv_Tex3a)*(tex2D(_SplatMap, IN.uv_SplatMap).b)) * _NormalHeight);
		//o.Normal += tex2D(_Tex2a, IN.uv_Tex2a).rgb*(tex2D(_SplatMap, IN.uv_SplatMap).g);
		//o.Normal += tex2D(_Tex3a, IN.uv_Tex3a).rgb*(tex2D(_SplatMap, IN.uv_SplatMap).b);
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		//o.Emissions = o.Albedo * _Color * .1;
	}
	ENDCG
	}
		FallBack "Diffuse"
}