Shader "Hidden/VignetteShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
		_K("K", vector) = (0,0,0,0)
	}
	
	CGINCLUDE

	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	half4 _K;

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
		fixed4 col = tex2D(_MainTex, i.uv);
		fixed4 vignette = lerp(1, 0, pow((distance(0.5, i.uv)+_K.x)*_K.y, _K.z));
		return col * vignette;
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
