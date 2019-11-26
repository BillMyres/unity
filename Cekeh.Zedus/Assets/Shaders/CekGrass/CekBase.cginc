// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

sampler2D _GroundTex, _GrassTex;
float _Size;

struct Input {
	float4 pos	: POSITION;
	fixed4 col	: COLOR;
	float3 nor	: NORMAL;
	float2 uv0	: TEXCOORD0;
};

Input vert(Input i) {
	Input o;

	o.pos = mul(unity_ObjectToWorld, i.pos);
	o.nor = i.nor;
	o.uv0 = i.uv0;
	o.col = fixed4(1,1,1,1);

	return o;
}

fixed4 frag(Input i) : COLOR{
	float4 uv0 = float4(i.uv0.xy, 0, 0);

	return i.col;
}

[maxvertexcount(4)]
void geo(triangle Input p[3], inout TriangleStream<Input> tri) {
	float4x4 ws = mul(UNITY_MATRIX_MVP, unity_WorldToObject);//World Space
	Input i;

	i.pos = p[0].pos;
	i.col = p[0].col;
	i.nor = p[0].nor;
	i.uv0 = p[0].uv0;
	tri.Append(i);

	i.pos = p[1].pos;
	i.col = p[1].col;
	i.nor = p[1].nor;
	i.uv0 = p[1].uv0;
	tri.Append(i);

	i.pos = p[2].pos;
	i.col = p[2].col;
	i.nor = p[2].nor;
	i.uv0 = p[2].uv0;
	tri.Append(i);

}