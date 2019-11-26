Shader "Unlit/Cekv2"{
	Properties{
		_GrassTex ("_GrassTex", 2D) = "white" {}
		_SplatMap("_SplatMap", 2D) = "white" {}
		_R("_R", 2D) = "white" {}
		_G("_G", 2D) = "white" {}
		_B("_B", 2D) = "white" {}

		_Scale("_Scale", Range(0, 10)) = 0.0
		_RenderDistance("_RenderDistance", Float) = 1.0
		_Angle("_Angle", Range(0, 360)) = 0.0
		_Radius("_Radius", Range(0, 1)) = 1.0
		_Density("_Density", Int) = 1
	}
	SubShader{
		Pass{
			AlphaToMask On
			//Cull Off

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma geometry geo

				#include "Cekv2Base.cginc"
			ENDCG
		}
		
		Pass{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "NSplatBase.cginc"
			ENDCG
		}
	}
}
