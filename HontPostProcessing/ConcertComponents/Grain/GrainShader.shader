Shader "Hidden/GrainShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
		_GrainTex ("_RgbTex (RGB)", 2D) = "" {}
		_Intensity("Intensity", float) = 0
	}
	
	CGINCLUDE

	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};
	
	sampler2D _MainTex;
	sampler2D _GrainTex;
	fixed _Intensity;
	fixed _Tile;

	half4 _MainTex_ST;
	
	float simSin(float t)
	{
		t = fmod(t, 1);
		t = 4 * (t - t * t);

		return t;
	}

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
		fixed4 noise = tex2D(_GrainTex, i.uv * _Tile + fixed2(simSin(_Time.x*100), simSin(_Time.y*100))) * _Intensity;
		return lerp(col, noise, noise.a);
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
