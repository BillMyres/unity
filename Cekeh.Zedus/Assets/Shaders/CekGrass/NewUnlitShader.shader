Shader "Unlit/CekGrass-Unlit"{
	Properties{
		_MainTex ("_MainTex", 2D) = "white" {}
		_SecdTex ("_SecdTex", 2D) = "white" {}
		_Size ("_Size", Range(0,3)) = 1.0
		_slidex ("_slidex", Range(-1,1)) = 0.0
		_slidez ("_slidez", Range(-1,1)) = 0.0
	}
	SubShader{
		Pass{
			AlphaToMask On
			Cull Off
//------------------------------------------------------------------------------------
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geo
			
			sampler2D _MainTex;
			float _Size, _slidex, _slidez;
			int index = 0;//0 = bottom, 1 = top;
			float rand(float2 coords);
			struct input{
				float2 uv0 : TEXCOORD0;
				float4 pos : POSITION;
				float3 nor : NORMAL;
			};

			input vert (input i){
				input o;
				o.pos = i.pos;
				o.uv0 = i.uv0;
				o.nor = i.nor;
				return o;
			}
			
			fixed4 frag (input i) : COLOR{
				fixed4 col = tex2D(_MainTex, i.uv0);
				return col;
			}

			[maxvertexcount(40)]
			void geo(triangle input p[3], inout TriangleStream<input> tri) {

				float4 x0 = p[0].pos,
					   x1 = p[1].pos,
					   x2 = p[2].pos;
				
				if (x1.x == x2.x) { index = 1; }
				else { index = 0; }

				float4 p0, p1, p2;

				float2 uv[3];
				if (index == 0) {
					uv[2] = float2(0, 0);//0
					uv[1] = float2(1, 0);//1
					uv[0] = float2(1, 1);//2

					p0 = p[2].pos;
					p1 = p[1].pos;
					p2 = p[0].pos;
					
				}else if (index == 1){
					uv[1] = float2(0, 0);//0
					uv[2] = float2(0, 1);//1
					uv[0] = float2(1, 1);//2

					p0 = p[1].pos;
					p1 = p[2].pos;
					p2 = p[0].pos;
				}

				float4x4 fly = UNITY_MATRIX_MVP;

				input i;
				
				//Grass Math, generate random point in center of triangle, with x = 0-10 && z = 0-10 (-choices left)
				//10 - (10 - right) choices (ie right=4, 10-4=6, 10-6=4 choices for forward)
				float4 stepR, stepF;
				
				if(index == 0){//bottom
					stepR = float4((p1.xyz - p0.xyz) / 10, 0);
					stepF = float4((p2.xyz - p1.xyz) / 10, 0);
				}
				if (index == 1) {//top
					stepR = -float4((p2.xyz - p1.xyz) / 10, 0);
					stepF = -float4((p1.xyz - p0.xyz) / 10, 0);
				}
				float4 up = float4(0, 1, 0, 0), right = float4(1, 0, 0, 0), forward = float4(0, 0, 1, 0);

				//for each blade
				for (int b = 1; b < 6; b++) {//1-6 because 0-5 would multiply by 0 , i don't want that.
					float r = (rand(b * p[2].pos.xz )* 4059 + b * 345)%10,//_slidex
						f = (rand(b * p[2].pos.xz) * 456 + b * 4523) % (10 - (10 - r));//forward = slider % choices //if (r == 0) { r = 1; }

					float4 target;
					if (index == 0) {
						target = p0;
					}
					else if (index == 1) {
						target = p2;
					}
					float4 v = target + (r * stepR) + (f * stepF) + float4(0, .01, 0, 0);//point to generate grass

					float3 look = cross(v, p[2].pos);
					look.y = 0;
					look = normalize(look);
					look = mul(fly, look);


					i = p[2];
					i.pos = mul(fly, v - stepR / 2);
					i.uv0 = float2(0, 0);
					tri.Append(i);

					i = p[2];
					i.pos = mul(fly, v + stepR / 2);
					i.uv0 = float2(1, 0);
					tri.Append(i);

					i = p[2];
					i.pos = mul(fly, v + float4(p[2].nor, 0) - stepR / 2 + stepR * _slidex + stepF * _slidez);
					i.uv0 = float2(0, 1);
					tri.Append(i);

					i = p[2];
					i.pos = mul(fly, v + float4(p[2].nor, 0) + stepR / 2 + stepR * _slidex + stepF * _slidez);
					i.uv0 = float2(1, 1);
					tri.Append(i);

					tri.RestartStrip();

					i = p[2];
					i.pos = mul(fly, v - stepF / 2);
					i.uv0 = float2(0, 0);
					tri.Append(i);

					i = p[2];
					i.pos = mul(fly, v + stepF / 2);
					i.uv0 = float2(1, 0);
					tri.Append(i);

					i = p[2];
					i.pos = mul(fly, v + float4(p[2].nor, 0) - stepF / 2 + stepR * _slidex + stepF * _slidez);
					i.uv0 = float2(0, 1);
					tri.Append(i);

					i = p[2];
					i.pos = mul(fly, v + float4(p[2].nor, 0) + stepF / 2 + stepR * _slidex + stepF * _slidez);
					i.uv0 = float2(1, 1);
					tri.Append(i);

					tri.RestartStrip();
				}
			}

			//RANDOM FUNCTION
			float rand(float2 coords) {
				return frac(sin(dot(coords.xy, float2(12.9898, 78.233))) * 43758.5453);
			}
			ENDCG
//------------------------------------------------------------------------------------
		}
	}
}
