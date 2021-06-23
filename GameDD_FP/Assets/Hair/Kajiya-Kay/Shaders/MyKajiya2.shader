﻿Shader "Unlit/MyKajiya2"
{
	Properties
	{
		_Diffuse("Diffuse", Color) = (1, 1, 1, 1)
		_Specular("Specular", Color) = (1, 1, 1, 1)
		_Gloss1("Gloss1", Range(8.0, 256)) = 20
		_Gloss2("Gloss2", Range(8.0, 256)) = 20
		_Shift1("Shift1", float) = 0
		_Shift2("Shift2", float) = 0
		_AlphaTex ("Alpha Tex", 2D) = "white" {}
		_tSpecShift("Shift Tex", 2D) = "white" {}
	}

		SubShader
	{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			fixed4 _Diffuse;
			fixed4 _Specular;
			float _Gloss1;
			float _Gloss2;
			float _Shift1;
			float _Shift2;
			float4 _AlphaTex_ST;
			sampler2D _AlphaTex;
			float4 _tSpecShift_ST;
			sampler2D _tSpecShift;

			struct a2v
			{
				float4 texcoord : TEXCOORD;
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float3 worldBinormal : TEXCOORD2;
				float4 uv : TEXCOORD3;
			};

			fixed3 ShiftTangent(fixed3 T, fixed3 N, fixed shift)
			{
				return normalize(T + shift * N);
			}

			fixed StrandSpecular(fixed3 T, fixed3 V, fixed3 L, fixed exponent)
			{
				fixed3 H = normalize(V + L);

				fixed dotTH = dot(T, H);
				fixed sinTH = sqrt(1 - dotTH * dotTH);
				fixed dirAtten = smoothstep(-1, 0, dotTH);

				return dirAtten * saturate(pow(sinTH, exponent));
			}

			//顶点着色器当中的计算
			v2f vert(a2v v)
			{
				v2f o;
				//转换顶点空间：模型=>投影
				o.pos = UnityObjectToClipPos(v.vertex);
				//转换顶点空间：模型=>世界
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				//转换法线空间：模型=>世界
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldNormal = worldNormal;
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				o.worldBinormal = cross(worldTangent, worldNormal);
				o.uv.xy = v.texcoord.xy * _tSpecShift_ST.xy + _tSpecShift_ST.zw;
				o.uv.zw = v.texcoord.xy * _AlphaTex_ST.xy + _AlphaTex_ST.zw;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed shiftTex = tex2D(_tSpecShift, i.uv)-0.5;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

				float3 B = normalize(i.worldBinormal);
				float3 N = normalize(i.worldNormal);
				float3 t1 = ShiftTangent(B, N, _Shift1 + shiftTex);
				float3 t2 = ShiftTangent(B, N, _Shift2 + shiftTex);
				float S1 = StrandSpecular(t1, viewDir, lightDir, _Gloss1);
				float S2 = StrandSpecular(t2, viewDir, lightDir, _Gloss2);

				//fixed3 specular = _LightColor0.rgb * _Specular.rgb * (S1 + S2 * _Diffuse.rgb);
				fixed3 specular = S1 * _Specular.rgb + S2 * _Specular.rgb;
				//Lanbert光照
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(i.worldNormal, lightDir));
				//对高光范围进行遮罩
				specular *= saturate(diffuse * 2);
				return fixed4(ambient + diffuse + specular, 1.0);
			}
			ENDCG
		}
	}
		FallBack "Specular"
}
