Shader "BurnTestShader" 
{
	//All of the properties the user will edit
	Properties 
	{
		////Define the textures we will need.

		//The Texture to be used during it's normal undissolved state. Unity Convention calles this _MainTex
		//_MainTex is the name of the variable. "Main Texture" is what it will be called in the inspecter. 2D tells the inspector to allow 2D texture. = white{} sets generic white texture as default value.
		_MainTex("Main Texture", 2D) = "white"{}
		//Set the dissolve shape, _DissolveMap, to take a 2D texture and set the default as White.
		_DissolveMap("Dissolve Shape", 2D) = "white"{}

		////Set up floats with a min/max to prevent user from exceeding boundaries
		//variables set up with range display as a slider in the inspector

		//These control the progress of the effect and size of the edge lines
		_DissolveVal("Dissolve Value", Range(-0.2, 1.2)) = 1.2
		_LineWidth("Line Width", Range(0.0, 0.2)) = 0.1

		//Color defines the colour of the edges of the effect
		_LineColor("Line Color", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	//the surface shader
	SubShader 
	{
		//redner objects using this shaderwhen renders geometry
		Tags{ "Queue" = "Transparent" }
		//define how transparency behaves
		Blend SrcAlpha OneMinusSrcAlpha

		//Tells the shader the code is written in CG
		CGPROGRAM
		//Unity should light our model with the lamber lighting model (diffuse lighting)
		#pragma surface surf Lambert

		//Define our texture variables
		sampler2D _MainTex;
		sampler2D _DissolveMap;
		//Define our nontexture variables
		float4 _LineColor;
		float _DissolveVal;
		float _LineWidth;

		//define what information we need to access about each vertex in teh model being shaded
		//all we need are the uv coordinates for each of the textures we are using
		struct Input
		{
			//grab the uvs of the model on the MainTexture
			half2 uv_MainTex;
			//grab the uvs of the model on the DissolveMap
			half2 uv_DissolveMap;
		};

		//boilerplate surface function. it takes the input defined above, and modieies a surface output struct for unity this will control what the fragment actually gets shaded as
		void surf(Input IN, inout SurfaceOutput o)
		{
			//set the color of a fragment
			//we’re looking for what colour is at the position in the texture defined by the uv for this position on the mesh. 
			//o.Albedo doesn’t set the alpha of our fragment, so we use .rgb to trim the alpha from this function. 
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

			half4 dissolve = tex2D(_DissolveMap, IN.uv_DissolveMap);

			//4 element vector with RGB and alpha set to zero
			half4 clear = half4(0.0, 0.0, 0.0, 0.0);

			//isClear resolves to 0 if dissolve.r isn't greater than dissolvVal + LineWidth
			int isClear = int(dissolve.r - (_DissolveVal + _LineWidth) + 0.99);
			//isAtLeastLine will be 0 if we should use the regular texture instead of using the line color or transparency
			int isAtLeastLine = int(dissolve.r - (_DissolveVal)+0.99);

			//Choose whether or not the alt color is clear or the line color, then choose whether t ouse the main texture, or the alt color
			half4 altCol = lerp(_LineColor, clear, isClear);
			o.Albedo = lerp(o.Albedo, altCol, isAtLeastLine);

			//Use alpha blending (as opposed to additive, or multiplicative transparency
			o.Alpha = lerp(1.0, 0.0, isClear);
		}

		//Tell the shader to stop writing code in CG
		ENDCG

	}
}