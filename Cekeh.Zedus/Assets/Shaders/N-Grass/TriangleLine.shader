// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Cekeh/TriangleLine"{
	Properties{
		_MainTex ("_MainTex", 2D) = "white" {}
	_Background("_Background", 2D) = "white" {}
		_Height("_Height", Range(0, 3)) = 0.0
			_Size("_Size", Range(0, 5)) = 0.0
			_SliderRight("_SliderRight", Range(0, 10)) = 0.0
			_SliderForward("_SliderForward", Range(0, 10)) = 0.0
	}
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass{
			Cull Off
//---------------------------------------
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma geometry geo

sampler2D _MainTex, _Background;
float _Height, _Size, _SliderRight, _SliderForward;
			
struct Input {
	fixed4 col : COLOR;
	float4 pos : POSITION;
	float3 nor : NORMAL; 
	float2 uv0 : TEXCOORD0;
};

Input vert(Input i) {
	Input o;
	o.pos = mul(unity_ObjectToWorld, i.pos);
	o.nor = i.nor;
	o.uv0 = i.uv0;
	o.col = float4(1,1,1,1);
	return o;
}

fixed4 frag(Input i) : COLOR {
	float4 uv0 = float4(i.uv0.xy, 0, 0);
	return i.col;
}
float rand(float2 coords) {
	return frac(sin(dot(coords.xy, float2(12.9898, 78.233))) * 43758.5453);
}

[maxvertexcount(6)]
void geo(triangle Input p[3], inout TriangleStream<Input> tri) {//point, line, triangle

	float4 right = float4(1, 0, 0, 0), up = float4(0, 1, 0, 0), forward = float4(0, 0, 1, 0);

	//float3 p0y = noise(p[0].pos.xz);

	float4 v[4];
	v[0] = float4(p[0].pos.xyz, 1);
	v[1] = float4(p[1].pos.xyz, 1);
	v[2] = float4(p[2].pos.xyz, 1);

	Input i;
	float4x4 vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);

	i.pos = mul(vp, v[0]);
	i.nor = p[0].nor;
	i.uv0 = float2(0, 0);
	i.col = tex2Dlod(_MainTex, float4(0, 0, 0, 1));
	tri.Append(i);

	i.pos = mul(vp, v[1]);
	i.nor = p[0].nor;
	i.uv0 = float2(1, 0);
	i.col = tex2Dlod(_MainTex, float4(1, 0, 0, 1));
	tri.Append(i);

	i.pos = mul(vp, v[2]);
	i.nor = p[0].nor;
	i.uv0 = float2(1, 1);
	i.col = tex2Dlod(_MainTex, float4(1, 0, 0, 1));
	tri.Append(i);

	tri.RestartStrip();

	float3 step = (v[0].xyz - v[2].xyz) / 10;
	float4 s = float4(step.xyz, 0);

	float4 slideUp, slideRight, slideForward;

	if (v[1].x > v[2].x) {//if bottom
		float4 slideUp = (s * _Size) * (forward);

		float4 rightStep = (v[1] - v[2]) / 10;
		float4 forwardStep = (v[0] - v[1]) / 10;


		float r = _SliderRight;//10 choices
		float f = _SliderForward % (10 - (10 - r));//10 - (10 - right) choices (ie right=4, 10-4=6, 10-6=4 choices for forward)
		//forward = slider % choices

		slideRight = r * rightStep;
		slideForward = f * forwardStep;

		float4 right = float4((v[1].xyz - v[2].xyz) / 10, 0), forward = float4((v[0].xyz - v[1].xyz) / 10, 0);
		float randx = (rand(right.xz * 945) * 34958 + _SliderRight) % 10, randz = (rand(forward.xz * 345) * 345834) % 10;
		float tz = (randz + _SliderForward) % (10 - (10 - randx));
		float4 target = v[2] + randx * right + tz * forward;

		i.pos = mul(vp, v[0]);
		i.nor = p[0].nor;
		i.uv0 = float2(0, 0);
		i.col = tex2Dlod(_MainTex, float4(0, 0, 0, 1));
		tri.Append(i);

		i.pos = mul(vp, v[2]);
		i.nor = p[0].nor;
		i.uv0 = float2(1, 0);
		i.col = tex2Dlod(_MainTex, float4(1, 0, 0, 1));
		tri.Append(i);

		float4 avg = v[2] + up * _Height + slideRight + slideForward;
		
		i.pos = mul(vp, target + float4(0,1,0,0));
		i.nor = p[0].nor;
		i.uv0 = float2(.5, 1);
		i.col = tex2Dlod(_MainTex, float4(.5, 0, 0, 1));
		tri.Append(i);
	}
	if (v[1].z < v[2].z) {//if top
		slideForward = (s.x * (_SliderForward)) * forward;
		slideRight = (s * (_SliderRight * 2)) * right;

		/*i.pos = mul(vp, v[2]);
		i.nor = p[0].nor;
		i.uv0 = float2(0, 0);
		tri.Append(i);

		i.pos = mul(vp, v[1]);
		i.nor = p[0].nor;
		i.uv0 = float2(1, 0);
		tri.Append(i);

		float4 avg = (v[1] + v[2]) / 2 + up * _Height + slideUp + slideRight + slideForward;
		i.pos = mul(vp, avg);
		i.nor = p[0].nor;
		i.uv0 = float2(0, 1);
		tri.Append(i);*/
	}

	/*i.pos = mul(vp, v[1]);
	i.nor = p[0].nor;
	i.uv0 = float2(0,0);
	tri.Append(i);

	i.pos = mul(vp, v[2]);
	i.nor = p[0].nor;
	i.uv0 = float2(1, 0);
	tri.Append(i);

	float4 avg = (v[1] + v[2]) / 2 + up * _Height + slideUp + slideRight + slideForward;
	i.pos = mul(vp, avg);
	i.nor = p[0].nor;
	i.uv0 = float2(0, 1);
	tri.Append(i);*/

	/*i.pos = mul(vp, v[3]);
	i.nor = p[0].nor;
	i.uv0 = p[0].uv0;
	tri.Append(i);*/

}

ENDCG
		}
	}
}
