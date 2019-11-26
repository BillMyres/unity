// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "GRASS/N-Grass"{
	Properties{
		_Scale("_Scale", Range(0,1)) = 1
		_Distance("_Distance", Range(10, 100)) = 50

		_MainTex ("_MainTex", 2D) = "white" {}
		_Size("_Size", Range(0,3)) = 0.5
		_SplatMap("_SplatMap", 2D) = "white" {}

		_R("_R", 2D) = "white" {}
		_G("_G", 2D) = "white" {}
		_B("_B", 2D) = "white" {}
	}
		SubShader{

		Pass{
		Name "NGrass"
			AlphaToMask On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geo

			sampler2D _SplatMap;
			sampler2D _MainTex;
			float _Size;
			float _Scale;
			float _Distance;

			float _TimeStamp;
			float _WindStrength;
			float3 _WindDirection;

			float3 _PlayerPosition;//set outside
			
			float rand(float2 coords);

			struct Input {
				float4 pos	: POSITION;
				float3 nor	: NORMAL;
				float2 uv0	: TEXCOORD0;
				fixed4 col  : COLOR;
			};

			Input vert(Input i) {
				//float4 position = mul();
				float random = rand(float2(i.pos.x, i.pos.z));
				float4 r = float4(	(random * 39847342) % 5 * .3, 
									random,
									(random * 53445589) % 5 * .3,
									0);
				//delete later for xz offset
				//r.xz = 0;

				Input o;
				//i.pos.y = 0;
				o.pos = mul(unity_ObjectToWorld, i.pos + r);
				o.nor = i.nor;
				o.uv0 = i.uv0;
				o.col = tex2Dlod(_SplatMap, float4(i.uv0.xy * _Scale, 0, 0));
				return o;
			}

			float rand(float2 coords) {
				return frac(sin(dot(coords.xy, float2(12.9898, 78.233))) * 43758.5453);
			}

			float4 frag(Input i) : COLOR{
				float4 uv = float4(i.uv0.xy, 0, 0);
				return tex2Dlod(_MainTex, uv);
			}

			[maxvertexcount(4)]
			void geo(point Input p[1], inout TriangleStream<Input> tri) {
				float size = 0.5 * _Size;

				float3 up = float3(0, 1, 0);
				float3 look = _WorldSpaceCameraPos - p[0].pos;
				look.y = 0;
				look = normalize(look);
				float3 right = cross(up, look);

				float3 nPos = p[0].pos;

				bool grass = false;
				float4 worldPos = mul(UNITY_MATRIX_MVP, p[0].pos);
				if (distance(worldPos.xyz, _PlayerPosition.xyz) < _Distance){
					if (p[0].col.g > 0.8) { grass = true; }
				}
				//if (normalize(tex2D(_SplatMap, float4(p[0].uv0.xy, 0, 0))).g) { grass = true; }
				float4 v[4];
				v[0] = float4(nPos + size * right - size * up, 1.0f);
				v[1] = float4(nPos + size * right + size * up, 1.0f);
				v[2] = float4(nPos - size * right - size * up, 1.0f);
				v[3] = float4(nPos - size * right + size * up, 1.0f);

				Input i;
				float4x4 vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);
				//if (tex2D(_SplatMap, p[0].uv0).g) {
				if (grass) {
					i.pos = mul(vp, v[0]);
					i.nor = p[0].nor;
					i.uv0 = float2(1, 0);
					i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
					tri.Append(i);

					i.pos = mul(vp, v[1]);
					i.nor = p[0].nor;
					i.uv0 = float2(1, 1);
					i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
					tri.Append(i);

					i.pos = mul(vp, v[2]);
					i.nor = p[0].nor;
					i.uv0 = float2(0, 0);
					i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
					tri.Append(i);

					i.pos = mul(vp, v[3]);
					i.nor = p[0].nor;
					i.uv0 = float2(0, 1);
					i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
					tri.Append(i);
				}
			}

			ENDCG
		}

		Pass{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};
		struct v2f {
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

		float _Scale;

		v2f vert(appdata v) {
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.uv * _Scale;
			UNITY_TRANSFER_FOG(o,o.vertex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target{
			fixed4 col = fixed4(((tex2D(_R, i.uv).rgb) * (tex2D(_SplatMap, i.uv).r)), 1);
		col += fixed4(((tex2D(_G, i.uv).rgb) * (tex2D(_SplatMap, i.uv).g)), 1);
		col += fixed4(((tex2D(_B, i.uv).rgb) * (tex2D(_SplatMap, i.uv).b)), 1);
		return col;
		}
			ENDCG
		}
	}
}
/*
Examples that helped... a lot :
http://http.developer.nvidia.com/GPUGems/gpugems_ch07.html
https://github.com/keijiro/KvantGrass

Projects that kept me motivated :
http://stijndelaruelle.com/pdf/grass.pdf
https://github.com/mreinfurt/Grass.DirectX

Learning to code in CG, these were great resources :
http://http.developer.nvidia.com/Cg/index_stdlib.html
http://http.developer.nvidia.com/CgTutorial/cg_tutorial_appendix_e.html
https://msdn.microsoft.com/en-us/library/windows/desktop/bb205122(v=vs.85).aspx

Still to look at :
http://outerra.blogspot.ca/2012/05/procedural-grass-rendering.html
*/