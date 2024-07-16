Shader "Draw/OutlineShader" {
	Properties{
		_OutlineColor("Outline Color", Color) = (1,0.95,0.74,0.75)
		_Outline("Outline width", Range(0.05,0.15)) = 0.2
		_NoiseTex("Noise Texture", 2D) = "white" {}
		_NoiseScale("Noise Scale", Range(1,10)) = 5
	}

		HLSLINCLUDE
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		sampler2D _NoiseTex;
		uniform float _Outline;
		uniform float4 _OutlineColor;
		uniform float _NoiseScale;

	struct Attributes {
		float4 positionOS : POSITION;
		float3 normalOS : NORMAL;
		float2 uv : TEXCOORD0;
	};

	struct Varyings {
		float4 positionHCS : SV_POSITION;
		float4 color : COLOR;
		float2 uv : TEXCOORD0;
	};

	float random(float2 st) {
		return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
	}

	Varyings vert(Attributes v) {
		Varyings o;

		// Apply outline scaling
		float3 scaledPosition = v.positionOS.xyz * (1.0 + _Outline);
		float4 positionWS = float4(scaledPosition, 1.0);

		// Transform to homogeneous clip space
		o.positionHCS = TransformObjectToHClip(positionWS);

		// Pass outline color to fragment shader
		o.color = _OutlineColor;

		o.uv = v.uv * _NoiseScale;

		return o;
	}

	half4 frag(Varyings i) : SV_Target{
		// Sample noise texture
		float noise = tex2D(_NoiseTex, i.uv).r;

		// Apply noise to outline color alpha
		half4 color = i.color;
		color.a *= noise;

		return color;
	}

		ENDHLSL

		SubShader {
		Tags{ "RenderType" = "Opaque" }
			Pass{
				Name "OUTLINE"
				Tags {"LightMode" = "UniversalForward"}
				Cull Front
				ZWrite On
				ColorMask RGB
				Blend SrcAlpha OneMinusSrcAlpha

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				ENDHLSL
		}
	}

	Fallback "Universal Render Pipeline/Lit"
}