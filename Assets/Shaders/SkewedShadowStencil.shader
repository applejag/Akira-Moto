// Adapted from the built-in Sprites-Default.shader
// You can download the source for the built-in shaders from:
// http://unity3d.com/unity/download/archive

// SOURCE: http://www.sector12games.com/skewshear-vertex-shader/

Shader "Custom/Skewed Shadow Stencil"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

		_HorizontalSkew ("Horizontal Skew", Float) = 0
		_VerticalSkew ("Vertical Skew", Float) = 0
	}

	SubShader
	{
		Stencil {
			Ref 1
			Comp Always
			Pass Replace
		}

		Tags
		{ 
			"Queue"="Geometry-1"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			sampler2D _MainTex;
			float _HorizontalSkew;
			float _VerticalSkew;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				
				// Create a skew transformation matrix
				float h = _HorizontalSkew;
				float v = _VerticalSkew;
				float4x4 transformMatrix = float4x4(
					1,h,0,0,
					v,1,0,0,
					0,0,1,0,
					0,0,0,1);
				
				float4 skewedVertex = mul(transformMatrix, IN.vertex);
				OUT.vertex = mul(UNITY_MATRIX_MVP, skewedVertex);

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, IN.texcoord);
				IN.color.a = c.a;
				IN.color.rgb *= c.a;

				if (c.a == 0)
					discard;

				return IN.color;
			}
		ENDCG
		}
	}
}