Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)

		_SplatMap("_SplatMap", 2D) = "white" {}

	_R("R", 2D) = "white" {}

	_G("G", 2D) = "white" {}

	_B("B", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct v2f{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _SplatMap;
			float4 _SplatMap_ST;
			sampler2D _R;
			float4 _R_ST;
			sampler2D _G;
			float4 _G_ST;
			sampler2D _B;
			float4 _B_ST;
			
			v2f vert (appdata v){
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target{
				fixed4 col = fixed4(((tex2D(_R, i.uv).rgb) * (tex2D(_SplatMap, i.uv).r)), 1);
					col += fixed4(((tex2D(_G, i.uv).rgb) * (tex2D(_SplatMap, i.uv).g)), 1);
					col += fixed4(((tex2D(_B, i.uv).rgb) * (tex2D(_SplatMap, i.uv).b)), 1);
				return col;
			}
			ENDCG
		}
	}
}
