Shader "Custom/FadeAwayLit"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Alpha ("Alpha", Range(0, 1)) = 1.0	}
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "LightMode" = "ForwardBase" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				//float2 uv : TEXCOORD0;
				LIGHTING_COORDS(0,1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				//o.uv = TRANSFORM_TEX(v.uv, _MainTex)
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				float att = LIGHT_ATTENUATION(i);
				return fixed4(1,0,0,1) * att;
			}
			ENDCG
		}
	}
}
