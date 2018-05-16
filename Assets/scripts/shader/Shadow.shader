// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Shadow"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white"{}
		_ShadowTex("Shadow", 2D) = "white"{}
		_Color("Color", Color) = (1, 1, 1, 1)
		_BackgroundColor("BackgroundColor", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _ShadowTex;
			fixed4 _Color;
			fixed4 _BackgroundColor;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 main = tex2D(_MainTex, i.uv);
				if (main.a > 0)
				{
					return main;
				}
				fixed4 c1 = fixed4(_Color.rgb, 1);
				fixed4 c2 = fixed4(_BackgroundColor.rgb, 1) * _BackgroundColor.a;
				fixed a1 = _Color.a * tex2D(_ShadowTex, i.uv).a;
				fixed a2 = 1 - a1;
				return c1 * a1 + c2 * a2;
			}
			ENDCG
		}
	}
}
