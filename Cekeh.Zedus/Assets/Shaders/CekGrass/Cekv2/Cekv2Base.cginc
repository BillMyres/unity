sampler2D _GrassTex, _SplatMap, _G;
float _Perlinx, _Perlinz, _Size, _Scale, _slidex, _slidez, _RenderDistance, _Angle, _Radius;
int index = 0, _Density;//0 = bottom, 1 = top;
float3 _PlayerPosition;

float rand(float2 coords);

struct input {
	float2 uv0 : TEXCOORD0;
	float2 uv1 : TEXCOORD1;
	float4 pos : POSITION;
	float3 nor : NORMAL;
	fixed4 col : COLOR;
};

input vert(input i) {
	input o;
	o.pos = i.pos;
	o.uv0 = i.uv0;
	o.uv1 = i.uv1;
	o.nor = i.nor;
	o.col = tex2Dlod(_SplatMap, float4(i.uv0.xy, 0, 0));
	return o;
}

fixed4 frag(input i) : COLOR{
	/*fixed4 col = fixed4(1,1,1,1);
	if (tex2D(_SplatMap, i.uv0).g > 0) {
		col = tex2D(_GrassTex, i.uv0);
	}*/
	fixed4 col = tex2D(_GrassTex, i.uv0) * tex2D(_G, i.uv1 * _Scale);
	return col;
}

[maxvertexcount(48)]
void geo(triangle input p[3], inout TriangleStream<input> tri) {
	
	float4 worldPos = mul(UNITY_MATRIX_MVP, p[0].pos);
	if (p[0].col.g <= 0 ||
		p[1].col.g <= 0 || 
		p[2].col.g <= 0 ||
		distance(worldPos.xyz, _PlayerPosition.xyz) > _RenderDistance) {
		return;
	}

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

	}
	else if (index == 1) {
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

	if (index == 0) {//bottom
		stepR = float4((p1.xyz - p0.xyz) / 10, 0);
		stepF = float4((p2.xyz - p1.xyz) / 10, 0);
	}
	if (index == 1) {//top
		stepR = -float4((p2.xyz - p1.xyz) / 10, 0);
		stepF = -float4((p1.xyz - p0.xyz) / 10, 0);
	}

	float4 up = float4(0, 1, 0, 0), right = float4(1, 0, 0, 0), forward = float4(0, 0, 1, 0);

	//for each blade
	for (int b = 1; b < 1 + _Density; b++) {//1-6 because 0-5 would multiply by 0 , i don't want that.
		float r = (rand(b * p[2].pos.xz) * 4059 + b * 345) % 10,//_slidex
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

		float angle = _Angle * ((rand(b * p[2].pos.xz) * 38474)%5);
		float radius = _Radius / 2;

		float pi = 3.141592653;

		//stepR = float4(1,0,0,0);
		float4 oneR = float4(1, 0, 0, 0),
			   oneF = float4(0, 0, 1, 0);

		float4 one = v, two = v;
		one.x = radius * cos((angle - 180) * pi / 180) + v.x;
		one.z = radius * sin((angle - 180) * pi / 180) + v.z;

		two.x = radius * cos(angle * pi / 180) + v.x;
		two.z = radius * sin(angle * pi / 180) + v.z;

		i = p[2];
		i.pos = mul(fly, one);
		i.uv0 = float2(0, 0);
		tri.Append(i);

		i = p[2];
		i.pos = mul(fly, two);
		i.uv0 = float2(1, 0);
		tri.Append(i);

		i = p[2];
		i.pos = mul(fly, one + float4(p[2].nor, 0) * (radius * 2) + stepR * _Perlinx + stepF * _Perlinz);
		i.uv0 = float2(0, 1);
		tri.Append(i);

		i = p[2];
		i.pos = mul(fly, two + float4(p[2].nor, 0) * (radius * 2) + stepR * _Perlinx + stepF * _Perlinz);
		i.uv0 = float2(1, 1);
		tri.Append(i);

		tri.RestartStrip();

		/*i = p[2];
		i.pos = mul(fly, v - oneF);
		i.uv0 = float2(0, 0);
		tri.Append(i);

		i = p[2];
		i.pos = mul(fly, v + oneF);
		i.uv0 = float2(1, 0);
		tri.Append(i);

		i = p[2];
		i.pos = mul(fly, v + float4(p[2].nor, 0) - oneF + stepR * _Perlinx + stepF * _Perlinz);
		i.uv0 = float2(0, 1);
		tri.Append(i);

		i = p[2];
		i.pos = mul(fly, v + float4(p[2].nor, 0) + oneF + stepR * _Perlinx + stepF * _Perlinz);
		i.uv0 = float2(1, 1);
		tri.Append(i);

		tri.RestartStrip();*/
		
	}
	
}

//RANDOM FUNCTION
float rand(float2 coords) {
	return frac(sin(dot(coords.xy, float2(12.9898, 78.233))) * 43758.5453);
}