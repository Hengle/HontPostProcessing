Shader "Hidden/CameraBlurShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "" {}
		_MainTex_TexSize("MainTex TexSize", vector) = (1,1,0,0)
		_BlurRadius("Blur Radius", float) = 0
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	fixed4 _MainTex_TexSize;
	fixed4 _Color;
	fixed _BlurRadius;

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 color = tex2D(_MainTex, i.uv);

		color.rgb *= 0.2f;

		// 模糊 -------

		float yOffset = _BlurRadius * 0.05f;
		float xOffset = _MainTex_TexSize.x / _MainTex_TexSize.y * yOffset;

		// 上
		float2 uvOffset = float2(0, yOffset);
		color.rgb += tex2D(_MainTex, i.uv + uvOffset).rgb * 0.1;

		// 下
		uvOffset.y = -yOffset;
		color.rgb += tex2D(_MainTex, i.uv + uvOffset).rgb * 0.1;

		// 左
		uvOffset.x = -xOffset;
		uvOffset.y = 0;
		color.rgb += tex2D(_MainTex, i.uv + uvOffset).rgb * 0.1;

		// 右
		uvOffset.x = xOffset;
		color.rgb += tex2D(_MainTex, i.uv + uvOffset).rgb * 0.1;

		// 右上
		uvOffset.x = xOffset * 0.707;
		uvOffset.y = yOffset * 0.707;
		color.rgb += tex2D(_MainTex, i.uv + uvOffset).rgb * 0.1;

		// 右下
		uvOffset.y = -uvOffset.y;
		color.rgb += tex2D(_MainTex, i.uv + uvOffset).rgb * 0.1;


		// 左下
		uvOffset.x = -uvOffset.x;
		color.rgb += tex2D(_MainTex, i.uv + uvOffset).rgb * 0.1;

		// 左上
		uvOffset.y = -uvOffset.y;
		color.rgb += tex2D(_MainTex, i.uv + uvOffset).rgb * 0.1;

		return color;
	}

	ENDCG

	Subshader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	Fallback off
}