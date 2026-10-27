Shader "FX/Aura Outline Ghost" {
	Properties{
		[Header(Aura)]

		_Color2("Aura Color", Color) = (.5,.5,.5,1)
		_ColorR("Rim Color", Color) = (.5,.5,.5,1)
		_Outline("Outline width", Range(.002, 0.8)) = .005
		_OutlineZ("Outline Z", Range(-.06, 0)) = -.05

		[Header(Aura Noise)]
		_NoiseTex("Noise Texture", 2D) = "white" { }
		_Scale("Noise Scale", Range(0.0, 0.05)) = 0.01
		_SpeedX("Speed X", Range(-10, 10)) = 0
		_SpeedY("Speed Y", Range(-10, 10)) = 3.0

		[Header(Aura Rim)]
		_Opacity("Noise Opacity", Range(0.01, 10.0)) = 10
		_Brightness("Brightness", Range(0.5, 20)) = 2
		_OutlineFixed("Outline Fixed Strength", Range(0, 5)) = 2
		_Edge("Rim Edge", Range(0.0, 1)) = 0.1
		_RimPower2("Rim Power", Range(0.01, 10.0)) = 3.0

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


		CGINCLUDE

#include "UnityCG.cginc"

			struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float4 ver : TEXCOORD3;
			UNITY_FOG_COORDS(0)
				float3 viewDir : TEXCOORD1;
			float3 normalDir : TEXCOORD2;

		};

		uniform float _Outline;
		uniform float _OutlineZ;// outline z offset
		uniform float _RimPower2;


		v2f vert(appdata v) {
			v2f o;

			float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, v.normal));

			//clipPosition.xyz += normalize(clipNormal) * _OutlineWidth;

			o.pos = UnityObjectToClipPos(v.vertex);
			o.ver = mul(unity_ObjectToWorld, v.vertex.xyz);
			float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
			float2 offset = TransformViewToProjection(norm.xy);
			o.pos.xy += offset * _Outline;
			o.pos.z += _OutlineZ;// push away from camera
			o.normalDir = clipNormal;
			o.normalDir = normalize(mul(float4(v.normal, 1), unity_WorldToObject).xyz); // normal direction
			o.viewDir = normalize(WorldSpaceViewDir(v.vertex)); // view direction
			UNITY_TRANSFER_FOG(o, o.pos);
			return o;
		}
		ENDCG

			SubShader{


			UsePass "Custom/GhostEffect/Empty"// empty depth pass
			UsePass "Custom/GhostEffect/FORWARD" // Base shader pass, using the standard ToonLit shader


			ZWrite Off
			Blend One One

			Pass{
			Name "OUTLINE"



			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma multi_compile_fog
	#pragma shader_feature FADE
	#pragma shader_feature INVERT

			sampler2D _NoiseTex;
			float _Scale, _Opacity, _Edge;
			uniform float4 _Color2, _ColorR;
			float _Brightness, _SpeedX, _SpeedY, _OffsetFade, _GlowBrightness, _OutlineFixed;

		fixed4 frag(v2f i) : SV_Target
		{

			// noise
			float speedx = _Time.x * _SpeedX;
			float speedy = _Time.x * _SpeedY;
			fixed4 n = tex2D(_NoiseTex, float2(i.pos.x * _Scale + speedx, i.pos.y * _Scale + speedy));
			// same noise, but bigger
			fixed4 n2 = tex2D(_NoiseTex, float2(i.pos.x * (_Scale * 0.5) + speedx, i.pos.y * (_Scale * 0.5) + speedy));
			// same but smaller
			fixed4 n3 = tex2D(_NoiseTex, float2(i.pos.x * (_Scale * 2) + speedx, i.pos.y * (_Scale * 2) + speedy));
			// combined
			float combinedNoise = (n.r * n2.r * 2) * n3.r * 2;

			float4 rim = pow(saturate(dot(i.viewDir, i.normalDir)), _RimPower2); // calculate inverted rim based on view and normal
			float rims = rim;
			rim -= combinedNoise; // subtract noise texture
			rim += (rims * _OutlineFixed);
			float4 texturedRim = saturate(rim.a * _Opacity); // make a harder edge
			float4 extraRim = (saturate((_Edge + rim.a) * _Opacity) - texturedRim) * _Brightness;// extra edge, subtracting the textured rim
			float4 result = (_Color2 * texturedRim) + (_ColorR * extraRim);// combine both with colors

			float fade = saturate((i.ver.y + _OffsetFade) * 2);
#if INVERT
			fade = 1 - fade;
#endif  
	#if FADE
			result *= fade;
	#endif
			UNITY_APPLY_FOG(i.fogCoord, result);
			return result;
		}
		ENDCG
		}
		}

			Fallback "Diffuse"
}