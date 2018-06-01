Shader "Hidden/OutlineEffectCoreShader"
{
	Properties
	{
		_OutlineColor("OutlineColor",color) = (1, 0, 0, 1)
		_MainTex("Base (RGB)", 2D) = "" {}
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half4 uv20 : TEXCOORD1;
		half4 uv21 : TEXCOORD2;
		half4 uv22 : TEXCOORD3;
		half4 uv23 : TEXCOORD4;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	half4 _MainTex_TexelSize;
	float4 _OutlineColor;
	sampler2D _OutlineMaskTexture;

	#define OFFSET 2

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		o.uv = v.texcoord;
		o.uv20.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy, _MainTex_ST);
		o.uv20.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(-OFFSET, -OFFSET), _MainTex_ST);

		o.uv21.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(OFFSET, -OFFSET), _MainTex_ST);
		o.uv21.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(-OFFSET, OFFSET), _MainTex_ST);

		o.uv22.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(OFFSET, 0), _MainTex_ST);
		o.uv22.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(-OFFSET, 0), _MainTex_ST);

		o.uv23.xy = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(0, OFFSET), _MainTex_ST);
		o.uv23.zw = UnityStereoScreenSpaceUVAdjust(v.texcoord + _MainTex_TexelSize.xy * half2(0, -OFFSET), _MainTex_ST);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 sourceColor = tex2D(_MainTex, i.uv);
		fixed4 color = tex2D(_OutlineMaskTexture, i.uv20.xy);
		color += tex2D(_OutlineMaskTexture, i.uv20.zw);
		color += tex2D(_OutlineMaskTexture, i.uv21.xy);
		color += tex2D(_OutlineMaskTexture, i.uv21.zw);
		color += tex2D(_OutlineMaskTexture, i.uv22.xy);
		color += tex2D(_OutlineMaskTexture, i.uv22.zw);
		color += tex2D(_OutlineMaskTexture, i.uv23.xy);
		color += tex2D(_OutlineMaskTexture, i.uv23.zw);

		color *= _OutlineColor;
		color.a = 1;

		fixed4 changedColor = lerp(sourceColor, color, color.r);

		return lerp(changedColor, sourceColor, tex2D(_OutlineMaskTexture, i.uv));
	}

	ENDCG

	Subshader
	{
		Pass
		{
			Name "Outline"
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	Fallback off
}
