Shader "Hidden/BloomShader"
{
	Properties
	{
		_MainTex("Base", 2D) = "" {}
		_BloomTex("Bloom", 2D) = "" {}
		_Streak_Length("Streak Length", float) = 1
		_Threshold("Threshold", float) = 0.1
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	struct v2f_XBlur
	{
		float4 pos : SV_POSITION;

		half4 uv_Slash1 : TEXCOORD0;
		half4 uv_Slash1_2 : TEXCOORD1;
		half4 uv_Slash1_3 : TEXCOORD2;
		half4 uv_Slash2 : TEXCOORD3;
		half4 uv_Slash2_2 : TEXCOORD4;
		half4 uv_Slash2_3 : TEXCOORD5;
	};
	
	struct v2f_Base
	{
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};

	uniform half4 _MainTex_TexelSize;
	half4 _MainTex_ST;
	sampler2D _MainTex;
	sampler2D _BloomTex;
	
	float _Streak_Length;
	float _Threshold;

	v2f_Base vert_Base(appdata_img v)
	{
		v2f_Base o = (v2f_Base)0;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		
		return o;
	}

	fixed4 frag_Base(v2f_Base i) : SV_Target
	{
		fixed4 color = tex2D(_MainTex, i.uv);

		fixed4 bloomColor = tex2D(_BloomTex, i.uv);
		
		return color + bloomColor;
	}
	
	v2f_Base vert_ExtractHDR(appdata_img v)
	{
		v2f_Base o = (v2f_Base)0;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		
		return o;
	}

	fixed4 frag_ExtractHDR(v2f_Base i) : SV_Target
	{
		return saturate(tex2D(_MainTex, i.uv.xy) - (1+_Threshold));
	}
	
	v2f_XBlur vert_XBlur(appdata_img v)
	{
		v2f_XBlur o = (v2f_XBlur)0;
		o.pos = UnityObjectToClipPos(v.vertex);

		float streak_Length = _Streak_Length;
		float streak_Length2 = _Streak_Length*2;
		float streak_Length3 = _Streak_Length*2.7;
		
		o.uv_Slash1.xy = v.texcoord.xy + half2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * streak_Length;
		o.uv_Slash1.zw = v.texcoord.xy + half2(-_MainTex_TexelSize.x, _MainTex_TexelSize.y) * streak_Length;
		
		o.uv_Slash2.xy = v.texcoord.xy + half2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * streak_Length;
		o.uv_Slash2.zw = v.texcoord.xy + half2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * streak_Length;
		
		o.uv_Slash1_2.xy = v.texcoord.xy + half2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * streak_Length2;
		o.uv_Slash1_2.zw = v.texcoord.xy + half2(-_MainTex_TexelSize.x, _MainTex_TexelSize.y) * streak_Length2;
		o.uv_Slash2_2.xy = v.texcoord.xy + half2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * streak_Length2;
		o.uv_Slash2_2.zw = v.texcoord.xy + half2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * streak_Length2;
		
		o.uv_Slash1_3.xy = v.texcoord.xy + half2(_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * streak_Length3;
		o.uv_Slash1_3.zw = v.texcoord.xy + half2(-_MainTex_TexelSize.x, _MainTex_TexelSize.y) * streak_Length3;
		o.uv_Slash2_3.xy = v.texcoord.xy + half2(-_MainTex_TexelSize.x, -_MainTex_TexelSize.y) * streak_Length3;
		o.uv_Slash2_3.zw = v.texcoord.xy + half2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * streak_Length3;
		
		return o;
	}

	fixed4 frag_XBlur(v2f_XBlur i) : SV_Target
	{
		fixed4 blurCol_Slash1xy = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash1.xy, _MainTex_ST));
		fixed4 blurCol_Slash1zw = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash1.zw, _MainTex_ST));
		
		fixed4 blurCol_Slash1xy_2 = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash1_2.xy, _MainTex_ST));
		fixed4 blurCol_Slash1zw_2 = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash1_2.zw, _MainTex_ST));
		
		fixed4 blurCol_Slash1xy_3 = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash1_3.xy, _MainTex_ST));
		fixed4 blurCol_Slash1zw_3 = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash1_3.zw, _MainTex_ST));
		
		fixed4 blurCol_Slash2xy = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash2.xy, _MainTex_ST));
		fixed4 blurCol_Slash2zw = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash2.zw, _MainTex_ST));
				
		fixed4 blurCol_Slash2xy_2 = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash2_2.xy, _MainTex_ST));
		fixed4 blurCol_Slash2zw_2 = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash2_2.zw, _MainTex_ST));

		fixed4 blurCol_Slash2xy_3 = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash2_3.xy, _MainTex_ST));
		fixed4 blurCol_Slash2zw_3 = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv_Slash2_3.zw, _MainTex_ST));
		
		fixed4 slash1 = (blurCol_Slash1xy + blurCol_Slash1zw + (blurCol_Slash1xy_2 + blurCol_Slash1zw_2)*0.5 + (blurCol_Slash1xy_3 + blurCol_Slash1zw_3)*0.2) / 3;
		fixed4 slash2 = (blurCol_Slash2xy + blurCol_Slash2zw + (blurCol_Slash2xy_2 + blurCol_Slash2zw_2)*0.5 + (blurCol_Slash2xy_3 + blurCol_Slash2zw_3)*0.2) / 3;
		
		return (slash1 + slash2) * 0.5;
	}

	ENDCG

	Subshader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert_Base
			#pragma fragment frag_Base
			ENDCG
		}
		
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert_XBlur
			#pragma fragment frag_XBlur
			ENDCG
		}
		
		Pass
		{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert_ExtractHDR
			#pragma fragment frag_ExtractHDR
			ENDCG
		}
	}
	Fallback off
}