// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Cekeh/ShowNormals"
{
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma geometry geo
			sampler2D _SplatMap, _MainTex;
float _Size, _Scale, _Distance, _TimeStamp, _WindStrength;
float3 _WindDirection, _PlayerPosition;//set outside

float rand(float2 coords);

struct Input {
	float4 pos	: POSITION;
	float3 nor	: NORMAL;
	float2 uv0	: TEXCOORD0;
	fixed4 col : COLOR;
};

Input vertBase(Input i, float2 seed) {
	float random = rand(float2(i.pos.x, i.pos.z));
	float ry = random * .1;
	if (ry > 0) { ry = -ry; }
	float4 r = float4(	(random * (seed.x)) % 10 * .2 - .5, 
						_Size/2, 
						(random * (seed.y)) % 10 * .2 - .5, 
						0);
	Input o;
	o.pos = mul(unity_ObjectToWorld, i.pos + r);
	o.nor = i.nor;
	o.uv0 = i.uv0;
	o.col = tex2Dlod(_SplatMap, float4(i.uv0.xy * _Scale, 0, 0));
	return o;
}

float rand(float2 coords) {
	return frac(sin(dot(coords.xy, float2(12.9898, 78.233))) * 43758.5453);
}

Input vert(Input i) {
	float2 seed = float2(945, 67);
	return vertBase(i, seed);
}

float4 frag(Input i) : SV_Target{
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
	if (distance(worldPos.xyz, _PlayerPosition.xyz) < _Distance) {
		if (p[0].col.g > 0.8) { grass = true; }
	}
	float3 r = float3(1,0,0);
	float3 u = float3(0,1,0);
	float4 v[4];
	v[0] = float4(nPos + size * r - size * u, 1.0f);
	v[1] = float4(nPos + size * r + size * u, 1.0f);
	v[2] = float4(nPos - size * r - size * u, 1.0f);
	v[3] = float4(nPos - size * r + size * u, 1.0f);

	Input i;
	float4x4 vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);
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
	}
}
