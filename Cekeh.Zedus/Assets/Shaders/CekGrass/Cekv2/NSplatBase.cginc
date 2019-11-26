



struct Input {
	float2 uv0 : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float4 vertex : POSITION;
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

Input vert(Input v) {
	Input o;
	o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv0 = v.uv0 * _Scale;
	o.uv1 = v.uv1;
	return o;
}

fixed4 frag(Input i) : SV_Target{
	fixed4 col = fixed4(((tex2D(_R, i.uv0).rgb) * (tex2D(_SplatMap, i.uv1).r)), 1);
	col += fixed4(((tex2D(_G, i.uv0).rgb) * (tex2D(_SplatMap, i.uv1).g)), 1);
	col += fixed4(((tex2D(_B, i.uv0).rgb) * (tex2D(_SplatMap, i.uv1).b)), 1);
	return col;
}

