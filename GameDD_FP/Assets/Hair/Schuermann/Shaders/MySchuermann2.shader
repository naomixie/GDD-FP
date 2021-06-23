Shader "Unlit/MySchuermann2"
{
	Properties
	{
		_Diffuse("Diffuse", Color) = (1, 1, 1, 1)
		_Specular("Specular", Color) = (1, 1, 1, 1)
		_Gloss1("Gloss1", Range(8.0, 256)) = 20
		_Gloss2("Gloss2", Range(8.0, 256)) = 20
		_Shift1("Shift1", float) = 0
		_Shift2("Shift2", float) = 0
		_tSpecShift("Shift Tex", 2D) = "white" {}
	}

		SubShader
	{
		Tags {"Queue" = "Geometry" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
		Pass
		{
		//1
		Blend Off
		Cull Back
		ZWrite on
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
				float2 uv : TEXCOORD3;
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
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldNormal = worldNormal;
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				o.worldBinormal = cross(worldTangent, worldNormal);
				o.uv = v.texcoord * _tSpecShift_ST.xy + _tSpecShift_ST.zw;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed shiftTex = tex2D(_tSpecShift, i.uv) - 0.5;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

				float3 B = normalize(i.worldBinormal);
				float3 N = normalize(i.worldNormal);
				float3 t1 = ShiftTangent(B, N, _Shift1 + shiftTex);
				float3 t2 = ShiftTangent(B, N, _Shift2 + shiftTex);
				float S1 = StrandSpecular(t1, viewDir, lightDir, _Gloss1);
				float S2 = StrandSpecular(t2, viewDir, lightDir, _Gloss2);

				fixed3 specular = S1 * _Specular.rgb + S2 * _Specular.rgb;
				//Lambert
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(i.worldNormal, lightDir));
				//对高光范围进行遮罩
				specular *= saturate(diffuse * 2);
				return fixed4(ambient + diffuse + specular, 1.0);
			}
			ENDCG

			//2
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		ZWrite off

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
				float2 uv : TEXCOORD3;
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
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldNormal = worldNormal;
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				o.worldBinormal = cross(worldTangent, worldNormal);
				o.uv = v.texcoord * _tSpecShift_ST.xy + _tSpecShift_ST.zw;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed shiftTex = tex2D(_tSpecShift, i.uv) - 0.5;

				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
				fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));

				float3 B = normalize(i.worldBinormal);
				float3 N = normalize(i.worldNormal);
				float3 t1 = ShiftTangent(B, N, _Shift1 + shiftTex);
				float3 t2 = ShiftTangent(B, N, _Shift2 + shiftTex);
				float S1 = StrandSpecular(t1, viewDir, lightDir, _Gloss1);
				float S2 = StrandSpecular(t2, viewDir, lightDir, _Gloss2);

				fixed3 specular = S1 * _Specular.rgb + S2 * _Specular.rgb;
				//Lambert
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(i.worldNormal, lightDir));
				//对高光范围进行遮罩
				specular *= saturate(diffuse * 2);
				return fixed4(ambient + diffuse + specular, 1.0);
			}
				ENDCG

	}
	}
		FallBack "Transparent/Cutout/VertexLit" //透明
}
