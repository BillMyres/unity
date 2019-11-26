// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

sampler2D _SplatMap, _MainTex, _G;
float _Size, _Scale, _Distance, _TimeStamp, _WindStrength, _Perlin;
float3 _WindDirection, _PlayerPosition;//set outside

float rand(float2 coords);

struct Input {
	float4 pos	: POSITION;
	float3 nor	: NORMAL;
	float2 uv0	: TEXCOORD0;
	float2 uv1	: TEXCOORD1;
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
	o.uv1 = i.uv1;
	o.col = tex2Dlod(_SplatMap, float4(i.uv0.xy * _Scale, 0, 0));
	return o;
}

float rand(float2 coords) {
	return frac(sin(dot(coords.xy, float2(12.9898, 78.233))) * 43758.5453);
}

fixed4 frag(Input i) : COLOR{
	float4 uv0 = float4(i.uv0.xy, 0, 0);
	float4 uv1 = float4(i.uv1.xy * _Scale, 0, 0);
	return tex2D(_MainTex, uv0) * tex2D(_G, uv1);
}

[maxvertexcount(8)]
void geo(triangle Input p[3], inout TriangleStream<Input> tri) {
	float size = 0.5 * _Size;

	float3 up = p[1].nor;
	float3 look = _WorldSpaceCameraPos - p[0].pos;
	look.y = 0;
	look = normalize(look);
	//float3 right = cross(up, look);
	float3 nPos = p[1].pos;

	bool grass = false;
	float4 worldPos = mul(UNITY_MATRIX_MVP, p[0].pos);
	if (distance(worldPos.xyz, _PlayerPosition.xyz) < _Distance) {
		if (p[0].col.g > 0.8) { grass = true; }
	}
	float3 r = float3(1,0,0);//right
	float3 u = normalize(p[0].pos.xyz * p[0].nor);//up
	float3 f = float3(0,0,1);//forward
	float random = rand(float2(p[0].pos.x, p[0].pos.z));
	float wx = rand(float2(nPos.x * 893, nPos.z * 102)),
		  wz = rand(float2(nPos.x * 392, nPos.z * 555));
	float3 wind = (float3(wx,0,wz) * random) * _Perlin / 2;

	//random x z position
	float4 WorldRight = float4(1,0,0,0), WorldForward = float4(0,0,1,0);

	float4 TriPoint[3];
	TriPoint[0] = float4(p[0].pos.xyz, 0);
	TriPoint[1] = float4(p[1].pos.xyz, 0);
	TriPoint[2] = float4(p[2].pos.xyz, 0);

	float4 right = float4((TriPoint[1].xyz - TriPoint[2].xyz) / 1000, 0), forward = float4((TriPoint[0].xyz - TriPoint[1].xyz) / 1000, 0);
	float randx = (rand(right.xz * 945) * 34958) % 10, randz = (rand(right.xz * 345) * 345834) % 10;
	float tz = randz % (10 - (10 - randx));
	float3 target = TriPoint[2] + randx * right + tz * forward;
	

	float4 v[8];
	v[0] = float4(target + size * r - size * up, 1.0f);
	v[1] = float4(target + size * r + size * up + wind, 1.0f);
	v[2] = float4(target - size * r - size * up, 1.0f);
	v[3] = float4(target - size * r + size * up + wind, 1.0f);

	v[4] = float4(target + size * f - size * up, 1.0f);
	v[5] = float4(target + size * f + size * up + wind, 1.0f);
	v[6] = float4(target - size * f - size * up, 1.0f);
	v[7] = float4(target - size * f + size * up + wind, 1.0f);

	Input i;
	float4x4 vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);
	if (grass) {
		i.pos = mul(vp, v[0]);
		i.nor = p[0].nor;
		i.uv0 = float2(1, 0);
		i.uv1 = p[0].uv1;
		i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
		tri.Append(i);

		i.pos = mul(vp, v[1]);
		i.nor = p[0].nor;
		i.uv0 = float2(1, 1);
		i.uv1 = p[0].uv1;
		i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
		tri.Append(i);

		i.pos = mul(vp, v[2]);
		i.nor = p[0].nor;
		i.uv0 = float2(0, 0);
		i.uv1 = p[0].uv1;
		i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
		tri.Append(i);

		i.pos = mul(vp, v[3]);
		i.nor = p[0].nor;
		i.uv0 = float2(0, 1);
		i.uv1 = p[0].uv1;
		i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
		tri.Append(i);
		
		tri.RestartStrip();

		i.pos = mul(vp, v[4]);
		i.nor = p[0].nor;
		i.uv0 = float2(1, 0);
		i.uv1 = p[0].uv1;
		i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
		tri.Append(i);

		i.pos = mul(vp, v[5]);
		i.nor = p[0].nor;
		i.uv0 = float2(1, 1);
		i.uv1 = p[0].uv1;
		i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
		tri.Append(i);

		i.pos = mul(vp, v[6]);
		i.nor = p[0].nor;
		i.uv0 = float2(0, 0);
		i.uv1 = p[0].uv1;
		i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
		tri.Append(i);

		i.pos = mul(vp, v[7]);
		i.nor = p[0].nor;
		i.uv0 = float2(0, 1);
		i.uv1 = p[0].uv1;
		i.col = tex2Dlod(_MainTex, float4(p[0].uv0.xy, 0, 0));
		tri.Append(i);
	}
}

//DIFFERENT SEEDS FOR EACH PASS **********************************
Input vert0(Input i) {
	float2 seed = float2(945, 67);
	return vertBase(i, seed);
}

Input vert1(Input i) {
	float2 seed = float2(10554, 240);
	return vertBase(i, seed);
}

Input vert2(Input i) {
	float2 seed = float2(35456, 644);
	return vertBase(i, seed);
}

Input vert3(Input i) {
	float2 seed = float2(456, 1006);
	return vertBase(i, seed);
}

Input vert4(Input i) {
	float2 seed = float2(12, 123565);
	return vertBase(i, seed);
}

Input vert5(Input i) {
	float2 seed = float2(112, 3456);
	return vertBase(i, seed);
}

Input vert6(Input i) {
	float2 seed = float2(2423, 4567);
	return vertBase(i, seed);
}

Input vert7(Input i) {
	float2 seed = float2(222, 3478);
	return vertBase(i, seed);
}

Input vert8(Input i) {
	float2 seed = float2(979, 2008);
	return vertBase(i, seed);
}

Input vert9(Input i) {
	float2 seed = float2(1994, 18);
	return vertBase(i, seed);
}
//****************************************************************
