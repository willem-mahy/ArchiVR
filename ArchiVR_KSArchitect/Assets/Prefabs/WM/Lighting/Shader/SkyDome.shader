Shader "Unlit/SkyDome"
{
	Properties
	{
		_SkyColor1("Sky Color 1", Color) = (0.1, 0.1, 1.0, 1.0)
		_SkyColor2("Sky Color 2", Color) = (0.5, 0.5, 1.0, 1.0)
		
		_LightDirection("Light Direction", Color) = (1.0, 0.0, 0.0, 0.0)

		_RenderGround("Render Ground", range(0, 1)) = 0
		_GroundColor("Ground Color", Color) = (0.1, 1.0, 0.1, 1.0)

		_FogIntensity("Fog Intensity", range(0, 1)) = 0
		_FogColor("Fog Color", Color) = (0.5, 0.5, 0.5, 0.5)

		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			//Blend N Add
			ZWrite Off
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 vertexObj : COLOR0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _SkyColor1;
			float4 _SkyColor2;
			float4 _GroundColor;
			float4 _LightDirection;
			float _RenderGround;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertexObj = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 finalColor = _GroundColor;

				fixed3 vertexObj = fixed3(i.vertexObj.r, i.vertexObj.g, i.vertexObj.b);
				fixed3 vertexObjNormalized = normalize(vertexObj);

				float renderGround = _RenderGround;

				if ((renderGround != 1) || (vertexObj.y > 0))
				{
					fixed3 lightDir = fixed3(_LightDirection.r, _LightDirection.g, _LightDirection.b);
					
					float f = 0.5 * (dot(vertexObjNormalized, lightDir) + 1.0);

					f = clamp(f, 0, 1);
					f = f * f * f;
					finalColor = lerp(_SkyColor1, _SkyColor2, f);

					// TODO: overlay cloud color?
				}

				// apply fog			
				UNITY_APPLY_FOG(i.fogCoord, finalColor)

				return finalColor;
			}
			ENDCG
		}
	}
}
