Shader "Custom/StandardSur" {
	Properties {
		_Scale("_Scale", Range(1, 5)) = 0.015625
		_Distance("_Distance", Range(10, 100)) = 50

		_MainTex ("_MainTex", 2D) = "white" {}
		_Size("_Size", Range(0,3)) = 0.5
		_SplatMap("_SplatMap", 2D) = "white" {}

		_R("_R", 2D) = "white" {}
		_G("_G", 2D) = "white" {}
		_B("_B", 2D) = "white" {}
	}
	SubShader{
			AlphaToMask On
			
			//Tags{ "LightMode" = "ForwardBase" }
		Pass{
			Cull Off
			CGPROGRAM
			
				#include "NGrassBeta.cginc"

				#pragma vertex vert0
				#pragma fragment frag
				#pragma geometry geo

			ENDCG
		}

		/*Pass{
			Cull Off
			CGPROGRAM

				#include "NGrassBase.cginc"

				#pragma vertex vert1
				#pragma fragment frag
				#pragma geometry geo

			ENDCG
		}

		Pass{
			Cull Off
			CGPROGRAM

				#include "NGrassBase.cginc"

				#pragma vertex vert2
				#pragma fragment frag
				#pragma geometry geo

			ENDCG
		}

		Pass{
			Cull Off
			CGPROGRAM

				#include "NGrassBase.cginc"

				#pragma vertex vert3
				#pragma fragment frag
				#pragma geometry geo

			ENDCG
		}

		Pass{
			Cull Off
			CGPROGRAM

				#include "NGrassBase.cginc"

				#pragma vertex vert4
				#pragma fragment frag
				#pragma geometry geo

			ENDCG
		}

		Pass{
			Cull Off
			CGPROGRAM

				#include "NGrassBase.cginc"

				#pragma vertex vert5
				#pragma fragment frag
				#pragma geometry geo

			ENDCG
		}*/

		Pass{
			CGPROGRAM

				#include "NSplatBase.cginc"

				#pragma vertex vert
				#pragma fragment frag


			ENDCG
		}
	}
}
