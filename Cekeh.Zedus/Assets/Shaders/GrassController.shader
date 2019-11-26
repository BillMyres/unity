Shader "Unlit/GrassController"
{
	Properties
	{
		_Color("_Color", Color) = (1,1,1,1)
		_Texture("_Texture", 2D) = "white" {}
		_Alpha("_Alpha", 2D) = "white" {}
		_Length("_Length", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{
			UsePass "101-Grass/0"
		}
		
	}
}
