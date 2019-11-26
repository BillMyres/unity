// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/FadeAway"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Alpha ("Alpha", Range(0, 1)) = 1.0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "LightMode" = "ForwardBase" }
		LOD 100

		//ZWrite Off
     	Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			//uniform float4 _LightColor0; 

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 col : COLOR0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Alpha;
			fixed4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				//lighting
				float4x4 modelMatrixInverse = unity_WorldToObject;
				float3 normalDirection = normalize(float3(mul(float4(v.normal, 0), modelMatrixInverse).xyz));
				float3 lightDirection = normalize(float3(_WorldSpaceLightPos0.xyz));
				float3 diffuseReflection = float3(_LightColor0.rgb) * max(0, dot(normalDirection, lightDirection));
				o.col = float4(diffuseReflection, 1) + UNITY_LIGHTMODEL_AMBIENT;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= _Color;

				col.a = _Alpha;
				return col * i.col;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}
