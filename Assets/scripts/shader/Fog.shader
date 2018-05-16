Shader "Hidden/Fog"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white"{}
		_Color("Color", Color) = (1, 1, 1, 1)
		_Scale("Scale", float) = 1
		_Offset("Offset", float) = 0
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
			fixed4 _Color;
			float _Scale;
			float _Offset;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 color = tex2D(_MainTex, i.uv);
				float a = color.a;
				//color = floor(color * 10) / 10;
				float2 viewport = i.uv - float2(0.5, 0.5);
				float aspect = _ScreenParams.x / _ScreenParams.y;
				viewport.x = viewport.x * aspect;
				float length = distance(float2(0, 0), viewport) * _Scale + _Offset;
				length = clamp(length, 0, 1);
				color = lerp(color, _Color - 0.05, length);
				color = lerp(_Color, color, a);
				return color;
			}
			ENDCG
		}
	}
}
