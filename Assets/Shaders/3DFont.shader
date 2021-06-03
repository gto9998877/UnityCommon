Shader "Custom/3DFont" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
//		_Glossiness ("Smoothness", Range(0,1)) = 0.5
//		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" // 支持透明
		"IgnoreProjector"="True"          // 不受Projector影响
		"Queue"="Transparent" }           // 渲染顺序位于 BackGround, Geometry, AlphaTest之后，Overlay之前
		LOD 200

		Pass {

			Material {
				Diffuse[_Color]
			}
			Blend SrcAlpha OneMinusSrcAlpha		
			Lighting Off		// 不受光照影响
			Cull Off			// 双面显示
			ZTest Always		// 总在最前
			ZWrite Off			// 不写深度
			Fog {Mode Off}		// 不受雾影响
			SetTexture[_MainTex]{
				constantColor[_Color]
				combine texture*constant
			}
		}
	}
	FallBack "Diffuse"
}
