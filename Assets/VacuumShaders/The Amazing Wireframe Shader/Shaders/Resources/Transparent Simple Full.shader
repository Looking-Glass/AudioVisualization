Shader "Hidden/VacuumShaders/The Amazing Wireframe/Mobile/Vertex Lit/Transparent/Full" 
{      
	Properties 
	{   
		//Tag         
		[V_WIRE_Tag] _V_WIRE_Tag("", float) = 0 
		
		//Rendering Options
		[V_WIRE_RenderingOptions_VertexLit] _V_WIRE_RenderingOptions_VertexLitEnumID("", float) = 0
		 

		//Visual Options
		[V_WIRE_Title] _V_WIRE_Title_V_Options("Visual Options", float) = 0  
		
		//Base 
		_Color("Color (RGB) Trans (A)", color) = (1, 1, 1, 1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white"{}			
		[V_WIRE_UVScroll] _V_WIRE_MainTex_Scroll("    ", vector) = (0, 0, 0, 0)
		
		//Vertex Color
		[V_WIRE_VertexColor] _V_WIRE_VertexColorEnumID ("", float) = 0	
		 
		 
		//Wire Options
		[V_WIRE_Title] _V_WIRE_Title_W_Options("Wire Options", float) = 0  
		
		[V_WIRE_HDRColor] _V_WIRE_Color("", color) = (0, 0, 0, 1)
		_V_WIRE_Size("Size", Range(0, 0.5)) = 0.05

		//Include Light
		[V_WIRE_IncludeLight] _V_WIRE_IncludeLightEnumID ("", float) = 0	
		    
		//Transparency 
		[V_WIRE_Transparency] _V_WIRE_TransparencyEnumID("", float) = 0 
		[HideInInspector]     _V_WIRE_TransparentTex("", 2D) = "white"{}		
		[HideInInspector]	  _V_WIRE_TransparentTex_Scroll("    ", vector) = (0, 0, 0, 0)
		[HideInInspector]	  _V_WIRE_TransparentTex_UVSet("    ", float) = 0
		[HideInInspector]	  _V_WIRE_TransparentTex_Invert("    ", float) = 0
		[HideInInspector]	  _V_WIRE_TransparentTex_Alpha_Offset("    ", Range(-1, 1)) = 0

		//Dynamic Mask
		[V_WIRE_Title]		 _V_WIRE_Title_M_Options("Dynamic Mask Options", float) = 0  
		[V_WIRE_DynamicMask] _V_WIRE_DynamicMaskEnumID("", float) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskInvert("", float) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskEffectsBaseTexEnumID("", int) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskEffectsBaseTexInvert("", float) = 0	
	} 
	    
	Category      
	{
		Tags { "Queue"="Transparent+1" 
		       "IgnoreProjector"="True"  
			   "RenderType"="Transparent"  
			 }    
		LOD 150 

		Blend SrcAlpha OneMinusSrcAlpha
	 
		SubShader  
		{			  
		 
			// Vertex Lit, emulated in shaders (4 lights max, no specular)
			Pass  
			{
				Tags { "LightMode" = "Vertex" }
				Lighting On 

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0


				#pragma multi_compile_fog

				#pragma shader_feature V_WIRE_LIGHT_OFF V_WIRE_LIGHT_ON
				#pragma shader_feature V_WIRE_TRANSPARENCY_OFF V_WIRE_TRANSPARENCY_ON

				#pragma shader_feature V_WIRE_VERTEX_COLOR_OFF V_WIRE_VERTEX_COLOR_ON

				#pragma shader_feature V_WIRE_DYNAMIC_MASK_OFF V_WIRE_DYNAMI_MASK_PLANE V_WIRE_DYNAMIC_MASK_SPHERE 
				#pragma shader_feature V_WIRE_DYNAMIC_MASK_BASE_TEX_OFF V_WIRE_DYNAMIC_MASK_BASE_TEX_ON 


				#define V_WIRE_TRANSPARENT
				#define V_WIRE_HAS_TEXTURE

				#include "../cginc/Wireframe_VertexLit.cginc"
				
				ENDCG
			}
		 
			// Lightmapped
			Pass 
			{
				Tags { "LightMode" = "VertexLM" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0


				#pragma multi_compile_fog

				#pragma shader_feature V_WIRE_LIGHT_OFF V_WIRE_LIGHT_ON
				#pragma shader_feature V_WIRE_TRANSPARENCY_OFF V_WIRE_TRANSPARENCY_ON

				#pragma shader_feature V_WIRE_VERTEX_COLOR_OFF V_WIRE_VERTEX_COLOR_ON

				#pragma shader_feature V_WIRE_DYNAMIC_MASK_OFF V_WIRE_DYNAMI_MASK_PLANE V_WIRE_DYNAMIC_MASK_SPHERE 
				#pragma shader_feature V_WIRE_DYNAMIC_MASK_BASE_TEX_OFF V_WIRE_DYNAMIC_MASK_BASE_TEX_ON 


				#define V_WIRE_LIGHTMAP_ON
				#define V_WIRE_TRANSPARENT
				#define V_WIRE_HAS_TEXTURE

				#include "../cginc/Wireframe_VertexLit.cginc"

				 
				ENDCG         
			}    
		     
			// Lightmapped, encoded as RGBM
			Pass 
	 		{
				Tags { "LightMode" = "VertexLMRGBM" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag 
				#pragma target 3.0


				#pragma multi_compile_fog 

				#pragma shader_feature V_WIRE_LIGHT_OFF V_WIRE_LIGHT_ON
				#pragma shader_feature V_WIRE_TRANSPARENCY_OFF V_WIRE_TRANSPARENCY_ON
				
				#pragma shader_feature V_WIRE_VERTEX_COLOR_OFF V_WIRE_VERTEX_COLOR_ON

				#pragma shader_feature V_WIRE_DYNAMIC_MASK_OFF V_WIRE_DYNAMI_MASK_PLANE V_WIRE_DYNAMIC_MASK_SPHERE 
				#pragma shader_feature V_WIRE_DYNAMIC_MASK_BASE_TEX_OFF V_WIRE_DYNAMIC_MASK_BASE_TEX_ON 
			  

				#define V_WIRE_LIGHTMAP_ON
				#define V_WIRE_TRANSPARENT
				#define V_WIRE_HAS_TEXTURE

				#include "../cginc/Wireframe_VertexLit.cginc"
				 
				ENDCG
			}  			
		}
	}

	FallBack Off
}
 
