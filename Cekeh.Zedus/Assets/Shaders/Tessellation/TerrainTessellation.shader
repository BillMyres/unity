Shader "TerrainTessellation"{
	Properties{
		_Tessellation ("_Tessellation", Range(1, 64)) = 1
		_Height ("_Height", Range(0, 1)) = 1
		_Width ("_Width", Range(0, 1)) = 0.12
		_Distance ("_Distance", Range(1, 100)) = 10
		_SplatMap ("_SplatMap", 2D) = "white" {}
		_GrassTex ("_GrassTex", 2D) = "white" {}

		_R("_R", 2D) = "white" {}
		_G("_G", 2D) = "white" {}
		_B("_B", 2D) = "white" {}
	}
	SubShader{
		Pass{
		AlphaToMask On
			CGPROGRAM
			#pragma vertex VS	//1
			#pragma hull HS		//2
			#pragma domain DS	//3
			#pragma geometry GS //4
			#pragma fragment FS	//5
			#include "Tessellation.cginc"

			float _Tessellation, _Distance, _Height, _Width, _Perlinx, _Perlinz;
			sampler2D _SplatMap, _GrassTex, _R, _G, _B;
			float rand(float2 coords);

	struct ABinput {
		float4 pos : POSITION;
		float3 nor : NORMAL;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD2;
		bool grass : TEXCOORD1;
		fixed4 col : COLOR;
	};

	struct SVinput {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD2;
		float3 nor : NORMAL;
		bool grass : TEXCOORD1;
		fixed4 col : COLOR;
		float num : TEXCOORD3;
	};

	struct TEinput {
		float TessFactor[3] : SV_TessFactor;
		float TessFactorIn : SV_InsideTessFactor;
	};

	struct FSoutput {
		fixed4 col : SV_Target0;
	};

			ABinput VS (ABinput i){	//Vertex-Shader to plug into the Hull-Shader
				ABinput o = i;
				o.col = tex2Dlod(_SplatMap, float4(i.uv0, 0, 0));
				return o;
			}

			TEinput HSconstant (InputPatch<ABinput, 3> i){	//Input 3 Verticies (triangle) for HSconstant (required for hull shader)
				float tess = UnityDistanceBasedTess(i[0].pos, i[1].pos, i[2].pos, 0, _Distance, _Tessellation);

				TEinput output = (TEinput)0;
				output.TessFactor[0] = output.TessFactor[1] = output.TessFactor[2] = tess;	//Set Tessellation
				output.TessFactorIn = tess;

				return output;
			}

			[domain("tri")]	//We're using triangles, see HSconstant ^
			[partitioning("integer")]
			[outputtopology("triangle_cw")] //Output will also be triangles (only one supported by DX11)
			[patchconstantfunc("HSconstant")]	//Set the constant we set up earlier (HSconstant)
			[outputcontrolpoints(3)]	//Again for the triange that will make up the output
			SVinput HS (InputPatch<ABinput, 3> i, uint uCPID : SV_OutputControlPointID){	//Hull-Shader / Tessellation-Shader for use with the Domain-Shader to determine the UV-COORDS
				SVinput output = (SVinput)0;

				output.pos = i[uCPID].pos;	//Set control points for the triangles
				output.uv0 = i[uCPID].uv0;
				output.nor = i[uCPID].nor;
				output.col = i[uCPID].col;

				return output;
			}

			[domain("tri")]	//Still working with triangles in the Domain-Shader
			SVinput DS (TEinput TSconstant, const OutputPatch<SVinput, 3> i, float3 uv : SV_DomainLocation){
				SVinput output = (SVinput)0;
				float3 pos = i[0].pos * uv.x + i[1].pos * uv.y + i[2].pos * uv.z;
				output.pos = float4(pos.xyz, 1);
				output.uv0 = i[0].uv0 * uv.x + i[1].uv0 * uv.y + i[2].uv0 * uv.z;
				//output.uv1 = i[0].uv1 * uv.x + i[1].uv1 * uv.y + i[2].uv1 * uv.z;
				output.nor = i[0].nor * uv.x + i[1].nor * uv.y + i[2].nor * uv.z;
				output.col = i[0].col * uv.x + i[1].col * uv.y + i[2].col * uv.z;
				//output.col = tex2Dlod(_SplatMap, float4(output.uv0, 0, 0));
				return output;
			}

			[maxvertexcount(7)]
			void GS(triangle SVinput i[3], inout TriangleStream<SVinput> tri) {	//Geometry-Shader!!! GRASS GALORE!
				SVinput p;

				float h20 = _Width;
				fixed4 white = fixed4(1, 1, 1, 1);
				fixed4 black = fixed4(0, 0, 0, 1);

				p = i[0];
				p.pos = mul(UNITY_MATRIX_MVP, p.pos);
				p.grass = false;
				//p.col = black;
				tri.Append(p);

				p = i[1];
				p.pos = mul(UNITY_MATRIX_MVP, p.pos);
				p.grass = false;
				//p.col = black;
				tri.Append(p);

				p = i[2];
				p.pos = mul(UNITY_MATRIX_MVP, p.pos);
				p.grass = false;
				//p.col = black;
				tri.Append(p);

				tri.RestartStrip();

				if (i[0].col.g > 0.5) {

					float d = (rand(float2(i[0].pos.x * 222, i[0].pos.z * 645) * 3890457)) % 10;

					float x = .5 - rand(float2(i[0].pos.x * 457, i[0].pos.z * 789));
					float z = .5 - rand(float2(i[0].pos.x * 999, i[0].pos.z * 396));

					float4 offset = float4(x * .5, 0, z * .5, 0);
					float4 highOffset = float4((x * .4) * _Perlinx, 0, (z * .4) * _Perlinz, 0);

					float4 right = float4(_Width, 0, 0, 0);
					float4 up = float4(0, _Height, 0, 0);

					float4 nor = float4(i[0].nor, 0);
					int num = (i[0].pos.x * 999 + i[0].pos.z * 135) % 21 + 1;

					p = i[0];
					p.pos += offset;
					p.pos = mul(UNITY_MATRIX_MVP, p.pos) + right;
					p.uv1 = float2(0, 0);
					p.grass = true;
					p.num = num;
					tri.Append(p);

					p = i[0];
					p.pos += offset;
					p.pos = mul(UNITY_MATRIX_MVP, p.pos) - right;
					p.uv1 = float2(1, 0);
					p.grass = true;
					p.num = num;
					tri.Append(p);

					p = i[0];
					p.pos += offset + highOffset + _Height * nor;
					p.pos = mul(UNITY_MATRIX_MVP, p.pos) + right;
					p.uv1 = float2(0, 1);
					p.grass = true;
					p.num = num;
					tri.Append(p);

					p = i[0];
					p.pos += offset + highOffset + _Height * nor;
					p.pos = mul(UNITY_MATRIX_MVP, p.pos) - right;
					p.uv1 = float2(1, 1);
					p.grass = true;
					p.num = num;
					tri.Append(p);
				}
			}

			FSoutput FS (SVinput i) {	//Fragment-Shader, will add the color or uv coords
				FSoutput output;
				if (i.grass) {//if white
					int r = (rand(float2(i.pos.x * 2342342 , i.pos.z * 30942043))) % 22;
					output.col = tex2D(_GrassTex, float2(i.uv1.x / 22 + ((.045454 * i.num)) , i.uv1.y));
					output.col.rgb *= tex2D(_G, i.uv0).rgb;
					//output.col = i.col;
				}
				if (!i.grass) {//if black
					output.col = fixed4(((tex2D(_R, i.uv0).rgb) * (tex2D(_SplatMap, i.uv0).r)), 1);
					output.col += fixed4(((tex2D(_G, i.uv0).rgb) * (tex2D(_SplatMap, i.uv0).g)), 0);
					output.col += fixed4(((tex2D(_B, i.uv0).rgb) * (tex2D(_SplatMap, i.uv0).b)), 0);
				}
				return output;
			}
			
			float rand(float2 coords) {
				return frac(sin(dot(coords.xy, float2(12.9898, 78.233))) * 43758.5453);
			}

			ENDCG
		}
	}
}
