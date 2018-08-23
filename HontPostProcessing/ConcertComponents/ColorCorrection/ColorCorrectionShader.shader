Shader "Hidden/ColorCorrectionShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
		_RgbTex ("_RgbTex (RGB)", 2D) = "" {}
	}
	
	CGINCLUDE

	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	sampler2D _RgbTex;
	fixed _Intensity;

	half4 _MainTex_ST;
	
	v2f vert( appdata_img v ) 
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = UnityStereoScreenSpaceUVAdjust(v.texcoord.xy, _MainTex_ST);
		return o;
	} 
	
	fixed4 frag(v2f i) : SV_Target 
	{
		fixed4 color = tex2D(_MainTex, i.uv);

		fixed mappingR = tex2D(_RgbTex, half2(color.r, 0.5)).r;
		fixed mappingG = tex2D(_RgbTex, half2(color.g, 0.5)).g;
		fixed mappingB = tex2D(_RgbTex, half2(color.b, 0.5)).b;
		
		color.rgb = lerp(color.rgb, fixed3(mappingR,mappingG,mappingB), _Intensity);
		return color;		
	}

	ENDCG 
	
Subshader {
 Pass {
	  ZTest Always Cull Off ZWrite Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
}

Fallback off
	
}
