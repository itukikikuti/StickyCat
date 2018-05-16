Shader "Sprites/Water"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white"{}
		_Color("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1, 1, 1, 1)
		[HideInInspector] _Flip("Flip", Vector) = (1, 1, 1, 1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white"{}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
		_Power("Power", Range(0, 1)) = 0.05
		_Speed("Speed", Float) = 0.05
		_Magnitude("Magnitude", Range(0, 1)) = 0.05
	}

	SubShader
	{
		Tags
		{ 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		GrabPass
		{
			"_GrabTexture"
		}

		Pass
		{
		CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"

			struct PixelData
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 uv2 : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _GrabTexture;
			float  _Power;
			float  _Speed;
			float  _Magnitude;
			
			PixelData Vert(appdata_t IN)
			{
				PixelData OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
	
				#ifdef UNITY_INSTANCING_ENABLED	
				IN.vertex.xy *= _Flip.xy;
				#endif

				IN.vertex.y += sin(_Time * _Speed + IN.vertex.x) * _Power;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color * _RendererColor;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				OUT.uv2 = ComputeScreenPos(float4(OUT.vertex.xyz / OUT.vertex.w, 1));

				return OUT;
			}

			fixed4 Frag(PixelData IN) : SV_Target
			{
				fixed4 bump = SampleSpriteTexture(IN.texcoord);
				float phase = _Time.z * 5;
				float offset = (IN.texcoord.x + IN.texcoord) * UNITY_TWO_PI;
				float r = Luminance(bump) * UNITY_TWO_PI;
				IN.uv2.xy += fixed2(cos(r + phase + offset), sin(r + phase + offset)) * _Magnitude;

				fixed4 col = tex2D(_GrabTexture, IN.uv2);
				if (col.a < 1)
				{
					col = IN.color * 0.5;
					col.a = 1;
				}
				else
					col *= IN.color;
				return col;
			}
		ENDCG
		}
	}
}
