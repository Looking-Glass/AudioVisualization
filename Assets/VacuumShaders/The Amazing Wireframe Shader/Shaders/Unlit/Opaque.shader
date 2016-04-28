// VacuumShaders 2015
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/The Amazing Wireframe/Mobile/Unlit"
{ 
    Properties     
    {	
		//Tag         
		[V_WIRE_Tag] _V_WIRE_Tag("", float) = 0 
		 
		//Rendering Options
		[V_WIRE_RenderingOptions_Unlit] _V_WIRE_RenderingOptions_UnlitEnumID("", float) = 0


		//Visual Options
		[V_WIRE_Title] _V_WIRE_Title_V_Options("Visual Options", float) = 0  
		
		//Base 
		_Color("Color (RGB)", color) = (1, 1, 1, 1)
		_MainTex("Base (RGB)", 2D) = "white"{}			
		[V_WIRE_UVScroll] _V_WIRE_MainTex_Scroll("    ", vector) = (0, 0, 0, 0)
		 		   
		//IBL
		[V_WIRE_IBL]      _V_WIRE_IBLEnumID("", float) = 0
		[HideInInspector] _V_WIRE_IBL_Cube_Intensity("", float) = 1
		[HideInInspector] _V_WIRE_IBL_Cube_Contrast("", float) = 1 
		[HideInInspector] _V_WIRE_IBL_Cube("", cube) = ""{}
		[HideInInspector] _V_WIRE_IBL_Light_Strength("", Range(-1, 1)) = 0	 
		[HideInInspector] _V_WIRE_IBL_Roughness("", Range(-1, 1)) = 0	   
		
		//Reflection 
		[V_WIRE_Reflection] _V_WIRE_ReflectionEnumID("", float) = 0
		[HideInInspector]	_Cube("", Cube) = ""{}  
		[HideInInspector]	_ReflectColor("", Color) = (0.5, 0.5, 0.5, 1)
		[HideInInspector]	_V_WIRE_Reflection_Strength("", Range(0, 1)) = 0.5
		[HideInInspector]	_V_WIRE_Reflection_Fresnel_Bias("", Range(-1, 1)) = -1
		[HideInInspector]	_V_WIRE_Reflection_Roughness("", Range(0, 1)) = 0.3
		 
		//Vertex Color  
		[V_WIRE_VertexColor] _V_WIRE_VertexColorEnumID ("", float) = 0	
		  
		               
		//Wire Options
		[V_WIRE_Title] _V_WIRE_Title_W_Options("Wire Options", float) = 0  		
		
		[V_WIRE_HDRColor] _V_WIRE_Color("", color) = (0, 0, 0, 1)
		_V_WIRE_Size("Size", Range(0, 0.5)) = 0.05
						  
		//Transparency 
		[V_WIRE_Transparency] _V_WIRE_TransparencyEnumID("", float) = 0 
		[HideInInspector]     _V_WIRE_TransparentTex("", 2D) = "white"{}		
		[HideInInspector]	  _V_WIRE_TransparentTex_Scroll("    ", vector) = (0, 0, 0, 0)
		[HideInInspector]	  _V_WIRE_TransparentTex_UVSet("    ", float) = 0
		[HideInInspector]	  _V_WIRE_TransparentTex_Invert("    ", float) = 0
		[HideInInspector]	  _V_WIRE_TransparentTex_Alpha_Offset("    ", Range(-1, 1)) = 0
				 
		//Fresnel
	    [V_WIRE_Fresnel]  _V_WIRE_FresnelEnumID ("Fresnel", Float) = 0	
		[HideInInspector] _V_WIRE_FresnelInvert("", float) = 0
		[HideInInspector] _V_WIRE_FresnelBias("", Range(-1, 1)) = 0
		[HideInInspector] _V_WIRE_FresnelPow("", Range(1, 16)) = 1

		//Dynamic Mask
		[V_WIRE_Title]		 _V_WIRE_Title_M_Options("Dynamic Mask Options", float) = 0  
		[V_WIRE_DynamicMask] _V_WIRE_DynamicMaskEnumID("", float) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskInvert("", float) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskEffectsBaseTexEnumID("", int) = 0
		[HideInInspector]    _V_WIRE_DynamicMaskEffectsBaseTexInvert("", float) = 0	
    }
	    
    SubShader 
    {    
		Tags { "RenderType"="Opaque" }   
			       
		Pass               
	    {			                
            CGPROGRAM       
		    #pragma vertex vert    
	    	#pragma fragment frag     
			#pragma target 3.0
			      
			#pragma multi_compile_fog  
			 
			#pragma shader_feature V_WIRE_IBL_OFF V_WIRE_IBL_ON 
			#pragma shader_feature V_WIRE_REFLECTION_OFF V_WIRE_REFLECTION_CUBE_SIMPLE V_WIRE_REFLECTION_CUBE_ADVANED V_WIRE_REFLECTION_UNITY_REFLECTION_PROBES
			#pragma shader_feature V_WIRE_VERTEX_COLOR_OFF V_WIRE_VERTEX_COLOR_ON

			#pragma shader_feature V_WIRE_TRANSPARENCY_OFF V_WIRE_TRANSPARENCY_ON
			#pragma shader_feature V_WIRE_FRESNEL_OFF V_WIRE_FRESNEL_ON

			#pragma shader_feature V_WIRE_DYNAMIC_MASK_OFF V_WIRE_DYNAMI_MASK_PLANE V_WIRE_DYNAMIC_MASK_SPHERE 
		  
			 
			#define V_WIRE_HAS_TEXTURE
			#define V_WIRE_OPAQUE 

			#include "../cginc/Wireframe_Unlit.cginc"
			ENDCG   

    	} //Pass			 
        
    } //SubShader

} //Shader
 
