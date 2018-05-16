Shader "Particles/Signal"
{
	Properties
	{
		_MainTex("Particle Texture", 2D) = "white"{}
		_InvFade("Soft Particles Factor", Range(0.01, 3)) = 1
	}

	Category
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}
		Blend One OneMinusSrcAlpha 
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off

		Subshader
		{
			GrabPass
			{
				"_GrabTexture"
			}

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_particles
				#include "UnityCG.cginc"

				sampler2D _MainTex;
				sampler2D _GrabTexture;
				fixed4 _TintColor;
				float4 _MainTex_ST;
				UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
				float _InvFade;

				struct appdata_t
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD1;
					#endif
					float2 grab : TEXCOORD2;
					float3 grab1 : TEXCOORD3;
					float3 grab2 : TEXCOORD4;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				float rand(float2 co)
				{
					return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
				}

				float noise(float2 pos)
				{
					float2 ip = floor(pos);
					float2 fp = smoothstep(0, 1, frac(pos));
					float4 a = float4(rand(ip + float2(0, 0)), rand(ip + float2(1, 0)), rand(ip + float2(0, 1)), rand(ip + float2(1, 1)));
					a.xy = lerp(a.xy, a.zw, fp.y);
					return lerp(a.x, a.y, fp.x);
				}

				float perlin(float2 pos)
				{
					return (noise(pos) + noise(pos * 2) + noise(pos * 4) + noise(pos * 8) + noise(pos * 16) + noise(pos * 32) ) / 6;
				}

				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos(o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.grab = ComputeScreenPos(float4(o.vertex.xyz / o.vertex.w, 1));
					float2 temp = float2(o.grab.x - o.color.a, (o.grab.y - o.color.a) * 0.1);
					float l = length(temp);
					float r = rand(float2(o.grab.x + o.color.a, o.grab.y)) * 0.3;
					float r1 = 0.02 * o.color.a;
					float r2 = r1 * 2;
    				float s1 = 1 - r1 * 2;
    				float s2 = 1 - r2 * 2;
					o.grab1 = float3(o.grab.x, o.grab.x * s1 + r1, o.grab.x * s2 + r2);
        			o.grab2 = float3(o.grab.y, o.grab.y * s1 + r1, o.grab.y * s2 + r2);
					o.vertex.x += noise(o.vertex.xy) * o.color.a;
					o.vertex.y += noise(o.vertex.yx) * o.color.a;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					#ifdef SOFTPARTICLES_ON
					float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
					float partZ = i.projPos.z;
					float fade = saturate(_InvFade * (sceneZ - partZ));
					i.color.a *= fade;
					#endif
					return fixed4(tex2D(_GrabTexture, float2(i.grab1.r, i.grab2.r)).r, tex2D(_GrabTexture, float2(i.grab1.g, i.grab2.g)).g, tex2D(_GrabTexture, float2(i.grab1.b, i.grab2.b)).b, 1);
				}
				ENDCG
			}
		}
	}
}
