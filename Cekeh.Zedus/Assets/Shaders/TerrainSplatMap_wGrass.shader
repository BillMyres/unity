Shader "00_TerrainSplatMap_wGrass"{
	Properties{
		_Color("_Color", Color) = (1,1,1,1)
		_Texture("_Texture", 2D) = "white" {}
		_Alpha("_Alpha", 2D) = "white" {}
		_Length("_Length", Range(0,0.1)) = 0.0
		_Layers("_Layers", Range(1,20)) = 1

		_WindSpeed("_WindSpeed", Range(0.0, 5.0)) = 0.0
		_WindDirectionx("_WindDirectionx", Range(0, 1)) = 0.0
		_WindDirectionz("_WindDirectionz", Range(0, 1)) = 0.0
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		//LOD 100
		
		UsePass "TerrainSplatMap_wGrass/addLayer"
		UsePass "TerrainSplatMap_wGrass/addLayer"
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{
		Name "addLayer"
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				sampler2D _Texture;
				sampler2D _Alpha;
				float4 _Color;
				float _Length;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;

					float4  col : COLOR;
				};
				vertexOutput vert(vertexInput v) {
					vertexOutput o;

					v.vertex.xyz += _Length * v.normal;

					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);

					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;
				}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
				fixed4 c1 = tex2D(_Alpha, v.uv1);

				c0.a -= c1 / 2;
				v.col = c0 * _Color;

				return v.col;
				}
			ENDCG
		}
	}
}
