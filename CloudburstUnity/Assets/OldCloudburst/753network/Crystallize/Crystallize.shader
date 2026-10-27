// Crystallize Shader v1.0 - Made by 753
// https://discord.gg/fGSneSe

Shader "753/Crystallize" {
	Properties
	{
		[NoScaleOffset]_Ramp("Ramp", 2D) = "white" {}
		[HDR]_Color("Color (HDR)", Color) = (1,1,1,1)
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
			};

			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			fixed4 _Color;
			uniform sampler2D _Ramp;

			fixed4 frag(v2f i) : SV_Target
			{
				float3 dpdx = ddx(i.posWorld);
				float3 dpdy = ddy(i.posWorld);
				i.normalDir = normalize(cross(dpdy, dpdx));

				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				float3 normalDirection = i.normalDir;
				float4 rampedTex = tex2D(_Ramp, abs(dot(normalDirection, viewDirection)));
				float3 emissive = (rampedTex * _Color.rgb);
				return fixed4(emissive, 1);
			}
			ENDCG
		}
	}
}