// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Cekeh/CekGrass" {
	Properties{
		_GroundTex("_GroundTex", 2D) = "white" {}
		_GrassTex("_GrassTex", 2D) = "white" {}
		_Size("_Size", Range(0,3)) = 0.5
	}
		SubShader{
		//AlphaToMask On

		Pass{
			Cull Off
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma geometry geo


			sampler2D _GroundTex, _GrassTex;
			float _Size;

			struct Input {
				float4 pos	: POSITION;
				fixed4 col : COLOR;
				float3 nor	: NORMAL;
				float2 uv0	: TEXCOORD0;
			};

			Input vert(Input i) {
				Input o;

				o.pos = mul(unity_ObjectToWorld, i.pos);
				o.nor = i.nor;
				o.uv0 = i.uv0;
				o.col = fixed4(1, 1, 1, 1);

				return o;
			}

			fixed4 frag(Input i) : COLOR{

				return i.col;
			}

		ENDCG
		}
	}
}
