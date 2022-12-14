Shader "ScreenSpace/Blur"
{
	Properties 
	{
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
		_BlurSize("Blur Size", float) = 0
		_CenterSize("Center Size", float) = 0
		_CenterBlur("Center Blur", float) = 0
		_Color("Color", Color) = (1,1,1,1)
	}

	CGINCLUDE
		#define SAMPLES 16

		half _BlurSize;
		half _CenterSize;
		half _CenterBlur;
		half4 _Color;

		sampler2D _MainTex;
		half4 _MainTex_TexelSize;
		
		struct appdata 
		{
			half4 pos : POSITION;
			half2 uv : TEXCOORD0;
		};
		
		fixed4 getBlur(appdata i, half2 direction) : COLOR
		{
			half d = length((i.uv - half2(0.5, 0.5)) / _MainTex_TexelSize.xy) - _CenterSize;
			d = min(max(d / length(_MainTex_TexelSize) * _CenterBlur, 0), 1);
			direction *= d * _MainTex_TexelSize.xy;

			half4 col = 0;
			half2 uv = i.uv - direction * SAMPLES * 0.5;
			for (half index = 0; index < SAMPLES; index++) {				
				col += tex2D(_MainTex, uv);
				uv += direction;
			}		
			col = col / SAMPLES;		

			return lerp(col, col * _Color, d * _BlurSize);
		}

		appdata vertPass(appdata v) 
		{
			v.pos.xy *= 2;
			v.pos.xy -= 1;
			return v;
		}

		fixed4 vBlur(appdata i) : COLOR
		{
			return getBlur(i, half2(0, _BlurSize));
		}

		fixed4 hBlur(appdata i) : COLOR
		{
			return getBlur(i, half2(_BlurSize, 0));
		}
	ENDCG

	SubShader 
	{
		ZTest Off Cull Off ZWrite Off Blend Off
		Fog { Mode off }

		Pass
		{
			CGPROGRAM
				#pragma vertex vertPass
				#pragma fragment vBlur
				#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}

		Pass
		{
			CGPROGRAM
				#pragma vertex vertPass
				#pragma fragment hBlur
				#pragma fragmentoption ARB_precision_hint_fastest
			ENDCG
		}
	}
}