// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "101-Grass" {
	Properties{
		_Color("_Color", Color) = (1,1,1,1)
		_Texture("_Texture", 2D) = "white" {}
		_Alpha("_Alpha", 2D) = "white" {}
		_Length("_Length", Range(0,0.1)) = 0.0

		_WindSpeed("_WindSpeed", Range(0.0,5.0)) = 1.0
		_WindDirectionx("_WindDirectionx", Range(0,1)) = 1.0
		_WindDirectionz("_WindDirectionz", Range(0,1)) = 0.5

		_Time00("_Time00", Range(-1,1)) = 0.1
		_SplatMap("_SplatMap", 2D) = "white" {}

		_Size("_Size", Range(0,3)) = 0.5
		_MainTex("_MainTex", 2D) = "white" {}
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					//v.vertex.xyz += (0) * v.normal;

					//v.vertex.x += _Time00 * (_WindDirectionx * _WindSpeed) * v.vertex.y;

					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1/2;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				sampler2D _SplatMap;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
					float2 uv2 : TEXCOORD2;
				};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
					float2 uv2 : TEXCOORD2;

					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					
					v.vertex.xyz += (_Length) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					o.uv2 = float4(v.uv2.xy, 0, 0);

					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

					Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					v.vertex.xyz += (_Length * 2) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					//o.uv0.x += _Time00;
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					v.vertex.xyz += (_Length * 3) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					//o.uv0.x += _Time00;
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					v.vertex.xyz += (_Length * 4) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					//o.uv0.x += _Time00;
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					v.vertex.xyz += (_Length * 5) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					//o.uv0.x += _Time00;
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					v.vertex.xyz += (_Length * 6) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					//o.uv0.x += _Time00;
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					v.vertex.xyz += (_Length * 7) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					//o.uv0.x += _Time00;
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					v.vertex.xyz += (_Length * 8) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					//o.uv0.x += _Time00;
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END

		Pass{//START
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				sampler2D _Texture;
				sampler2D _Alpha;
				uniform float4 _Color;
				uniform float _Length;
				float _WindSpeed;
				float _WindDirectionx;
				float _WindDirectionz;

				float _Time00;

				struct vertexInput {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;};
				struct vertexOutput {
					float4 pos : SV_POSITION;
					float3 normal : NORMAL;
					float2 uv0 : TEXCOORD0;
					float2 uv1 : TEXCOORD1;
				
					float4  col : COLOR;};

				vertexOutput vert(vertexInput v) {
					vertexOutput o;
					v.vertex.xyz += (_Length * 9) * v.normal;
					float4 m = mul(UNITY_MATRIX_MVP, v.vertex);
					float wdx = _WindDirectionx * (((m.x * 9834) + (m.z * 3443)) % 2);
					float wdz = _WindDirectionz * (((m.x * 4536) + (m.z * 9843)) % 2);
					v.vertex.x += _Time00 * (wdx * _WindSpeed) * v.vertex.y;
					v.vertex.z += _Time00 * (wdz * _WindSpeed) * v.vertex.y;
					//o.uv0.x += _Time00;
					o.uv0 = float4(v.uv0.xy, 0, 0);
					o.uv1 = float4(v.uv1.xy, 0, 0);
					
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;}
				float4 frag(vertexOutput v) : COLOR{
					fixed4 c0 = tex2D(_Texture, v.uv0);
					fixed4 c1 = tex2D(_Alpha, v.uv1);

					c0.a -= c1;
					v.col = c0 * _Color;

					return v.col;}
			ENDCG
		}//END
			Pass{
						AlphaToMask On
						CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma geometry geo


						sampler2D _MainTex;
					float _Size;

					float _TimeStamp;
					float _WindStrength;
					float3 _WindDirection;

					float rand(float2 coords);

					struct Input {
						float4 pos	: POSITION;
						float3 nor	: NORMAL;
						float2 uv0	: TEXCOORD0;
					};

					Input vert(Input i) {
						//float3 translation = CalcTranslation(i.pos, _TimeStamp, _WindDirection, _WindStrength);
						//float r1 = rand(i.pos.xz);
						float4 r = float4(rand(float2(i.pos.xz)) - _Size, -(rand(float2(i.pos.yz))) % 10 + _Size / 2, rand(float2(i.pos.xz)) - _Size, 0);

						Input o;
						i.pos.y = 0;
						o.pos = mul(unity_ObjectToWorld, i.pos + r);
						//o.pos.y = 0;
						o.nor = i.nor;
						o.uv0 = i.uv0;

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

						float4 v[4];
						v[0] = float4(nPos + size * right - size * up, 1.0f);
						v[1] = float4(nPos + size * right + size * up, 1.0f);
						v[2] = float4(nPos - size * right - size * up, 1.0f);
						v[3] = float4(nPos - size * right + size * up, 1.0f);

						Input i;
						float4x4 vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);

						i.pos = mul(vp, v[0]);
						i.nor = p[0].nor;
						i.uv0 = float2(1, 0);
						tri.Append(i);

						i.pos = mul(vp, v[1]);
						i.nor = p[0].nor;
						i.uv0 = float2(1, 1);
						tri.Append(i);

						i.pos = mul(vp, v[2]);
						i.nor = p[0].nor;
						i.uv0 = float2(0, 0);
						tri.Append(i);

						i.pos = mul(vp, v[3]);
						i.nor = p[0].nor;
						i.uv0 = float2(0, 1);
						tri.Append(i);

						/*i.pos = mul(vp, v[4]);
						i.nor = p[1].nor;
						i.uv0 = p[1].uv0;
						tri.Append(i);

						i.pos = mul(vp, v[5]);
						i.nor = p[2].nor;
						i.uv0 = p[2].uv0;
						tri.Append(i);

						i.pos = mul(vp, v[6]);
						i.nor = p[0].nor;
						i.uv0 = p[0].uv0;
						tri.Append(i);*/
					}

					ENDCG
					}
	}
}