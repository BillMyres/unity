Shader "T-Splat" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)

		_SplatMap("_SplatMap", 2D) = "white" {}

	_Rock("R", 2D) = "white" {}

	_Grass("G", 2D) = "white" {}

	_Sand("B", 2D) = "white" {}
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		//Pass{
			CGPROGRAM
				#pragma surface surf Standard fullforwardshadows
				#pragma target 4.0
				struct Input {
					float2 _Rock;
					float2 _Grass;
					float2 _Sand;
					float2 uv_SplatMap;
				};
				sampler2D _Rock;
				sampler2D _Grass;
				sampler2D _Sand;

				sampler2D _SplatMap;

				void surf(Input IN, inout SurfaceOutputStandard o) {
					o.Albedo += ((tex2D(_Rock, IN.uv_SplatMap).rgb)*(tex2D(_SplatMap, IN.uv_SplatMap).r));
					o.Albedo += ((tex2D(_Grass, IN.uv_SplatMap).rgb)*(tex2D(_SplatMap, IN.uv_SplatMap).g));
					o.Albedo += ((tex2D(_Sand, IN.uv_SplatMap).rgb)*(tex2D(_SplatMap, IN.uv_SplatMap).b));
				}
			ENDCG
		//}
	}
		FallBack "Diffuse"
}