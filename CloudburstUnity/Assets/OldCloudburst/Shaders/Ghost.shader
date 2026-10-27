Shader "Custom/GhostEffect"
{
	Properties
	{
		[Header(Basics)]
		_Color("Texture Color", Color) = (0,0,0,0)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		[Toggle(NORMALS)] _NORMALS("Use Normals", Float) = 0
		_Normal("Normal", 2D) = "bump" {}

		[Header(Extra Emission Texture)]
		_Mask("Emission Mask (R)", 2D) = "black" {}
		_EmisIntensity("Emission Mask Intensity", Range(0,8.0)) = 3.0
		_RimColorMask("Rim Color Emis Mask", Color) = (0,0.19,0.16,0.0)

		[Header(Noise)]
		_Noise("Noise Texture", 2D) = "white" {}
		_NScale("Noise Scale", Range(0,10)) = 1
		_Speed("Speed", Range(-10, 10.0)) = 1

		[Header(Base Rim)]
		_RimColor("Rim Color", Color) = (0,0.8,0.8,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
		_GlowBrightness("Glow Brightness", Range(0.01, 30.0)) = 3.0

		[Header(Fade)]
		[Toggle(FADE)] _FADE("Fade To Bottom", Float) = 1
		[Toggle(INVERT)] _INVERT("Invert Fade", Float) = 0
		_OffsetFade("Offset Fade", Range(-10, 10.0)) = 1

		[Header(Cutoff)]
		[Toggle(HARD)] _HARD("Cutoff Rim", Float) = 0
		_Cutoff("Cutoff", Range(0, 1)) = 0.5
		_Smooth("Smoothness", Range(-1,1)) = 0.1

		[Header(Blending)]
		[Enum(UnityEngine.Rendering.BlendMode)]  _SrcFactor("Source Factor", Int) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstFactor("Destination Factor", Int) = 1

	}
		SubShader
		{
			Tags{ "Queue" = "Transparent" }
			LOD 200
			// Blending based on enums in properties
			Blend[_SrcFactor][_DstFactor]

			Pass{
			Name "Empty"
			 ZWrite On
			ColorMask 0
		}



			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
	#pragma surface surf ToonRamp keepalpha vertex:vert

			// Use shader model 3.0 target, to get nicer looking lighting
	#pragma target 3.0
	#pragma shader_feature FADE
	#pragma shader_feature NORMALS
	#pragma shader_feature INVERT
	#pragma shader_feature HARD
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif
		float d = dot(s.Normal, lightDir);
		float dChange = fwidth(d);
		float3 lightIntensity = smoothstep(0 , dChange + 0.05, d);

		half4 c;
		atten *= smoothstep(fwidth(d),d, 0.5);
		c.rgb = s.Albedo * _LightColor0.rgb * lightIntensity * (atten * 2);
		//c.a = 0; we don't want the alpha
		c.a = s.Alpha; // use the alpha of the surface output
		return c;
	}

		struct Input
		{
			float2 uv_MainTex;
			float2 uv2_Noise;
			float2 uv_Normal;
			float3 viewDir;
			float4 objPos;
		};


		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.objPos = mul(unity_ObjectToWorld, v.vertex.xyz);
		}

		float4 _Color, _RimColor, _RimColorMask;
		float _RimPower, _NScale;
		sampler2D _MainTex, _Noise, _Mask, _Normal;
		float _OffsetFade, _Speed, _GlowBrightness, _EmisIntensity;
		float _Cutoff, _Smooth;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutput o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// red mask for extra emission
			fixed4 mask = tex2D(_Mask, IN.uv_MainTex);

			// noise thats scaled and moves over time
			float scrollSpeed = _Time.x * _Speed;
			fixed4 n = tex2D(_Noise, IN.uv2_Noise * _NScale + scrollSpeed);
			// same noise, but bigger
			fixed4 n2 = tex2D(_Noise, IN.uv2_Noise * (_NScale * 0.5) + scrollSpeed);
			// same but smaller
			fixed4 n3 = tex2D(_Noise, IN.uv2_Noise * (_NScale * 2) + scrollSpeed);
			// combined
			float combinedNoise = (n.r * n2.r * 2) * n3.r * 2;
	#if NORMALS
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));
	#endif
			//rim lighting with noise
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir + (combinedNoise * 2)), o.Normal));
			rim = pow(rim, _RimPower);
	#if HARD
			rim = smoothstep(_Cutoff,_Cutoff + _Smooth, rim);
	#endif
			// add color
			float3 coloredRim = rim * _RimColor;
			// colored emission mask
			float3 coloredEmisMask = _RimColorMask * mask.r * _EmisIntensity;
			// combined
			o.Emission = coloredRim + coloredEmisMask;
			// create gradient fade over object position
			float fade = saturate(IN.objPos.y + _OffsetFade);

	#if INVERT
			fade = 1 - fade;
	#endif  
	#if FADE
			// fade rim over object
			rim *= fade;
			// add fade for a bit more strength on the end
			rim += fade / 10;
			// add color
			float3 coloredRimFade = pow(rim, _RimPower) * rim * _RimColor;
			// colored emission mask
			float3 coloredEmisMaskFade = _RimColorMask * mask.r * fade * _EmisIntensity;
			// combined
			o.Emission = coloredRimFade + coloredEmisMaskFade;
	#endif
			// add some extra glow if needed
			o.Emission *= _GlowBrightness;
		}
		ENDCG

		}
			FallBack "Diffuse"
}