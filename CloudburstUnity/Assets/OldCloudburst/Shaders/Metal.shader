Shader "Toon/Lit Metal" {
	Properties{
		_Color("Base Color", Color) = (0.8,0.4,0.15,1)
		_Ramp("Toon Ramp (RGB)", 2D) = "white" {}
		[Header(Metal)]
		_Brightness("Specular Brightness", Range(0, 2)) = 1.3
		_Offset("Specular Size", Range(0, 1)) = 0.8 //  
		_SpecuColor("Specular Color", Color) = (0.8,0.45,0.2,1)
		[Header(Highlight)]
		_HighlightOffset("Highlight Size", Range(0, 1)) = 0.9
		_HiColor("Highlight Color", Color) = (1,1,1,1)
		[Header(Rim)]
		_RimColor("Rim Color", Color) = (1,0.3,0.3,1)
		_RimPower("Rim Power", Range(0, 20)) = 6
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

	CGPROGRAM
	#pragma surface surf ToonRamp vertex:vert

	sampler2D _Ramp;

		// custom lighting function that uses a texture ramp based
		// on angle between light direction and normal
		#pragma lighting ToonRamp exclude_path:prepass
		inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif

			half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
			half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			c.a = 0;
			return c;
		}


		float4 _Color;
		float _Offset;
		float4 _HiColor;
		float _HighlightOffset;
		float _Brightness;
		float4 _SpecuColor;
		float4 _RimColor;
		float _RimPower;

		struct Input {
			float2 uv_MainTex : TEXCOORD0;
			float3 viewDir;
			float3 lightDir;
		};

		 void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.lightDir = WorldSpaceLightDir(v.vertex); // get the worldspace lighting direction
			}


		void surf(Input IN, inout SurfaceOutput o) {
			float spec = dot(normalize(IN.viewDir + IN.lightDir), o.Normal);// specular based on view and light direction
			float cutOff = step(spec, _Offset); // cutoff for where base color is
			float3 baseAlbedo = _Color * cutOff;// base color
			float3 specularAlbedo = _SpecuColor * (1 - cutOff) * _Brightness;// inverted base cutoff times specular color
			float highlight = saturate(dot(IN.lightDir, o.Normal)); // highlight based on light direction
			float4 highlightAlbedo = (step(_HighlightOffset,highlight) * _HiColor); //glowing highlight
			o.Albedo = baseAlbedo + specularAlbedo + highlightAlbedo;// result
			half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));// standard rim calculation   
			o.Emission += _RimColor.rgb * pow(rim, _RimPower);// rim lighting added to glowing highlight


		}
		ENDCG

		}

			Fallback "Diffuse"
}