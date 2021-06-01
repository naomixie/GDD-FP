Shader "Unlit/LightShader"
{
    Properties
    {
        // Properties中声明的语法是：变量名（“显示名”，类型） = 默认值
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        // LOD 100

        Pass
        {
            CGPROGRAM
            //为了在程序中使用变量，我们需要在CGPROGRAM ... ENDCG语块中再次声明相同名字的变量
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct VertexData{
                //我们只需要在 VertexData 中定义一个 float4 类型的变量 position ，并指定它的
                //语义为 POSITION 。这样一来，程序便会让 VertexData.position 被解释为顶点的位置信息。
                float4 position: POSITION;
                float2 uv: TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct FragmentData{
                //为这个变量指定语义为 SV_POSITION ，表示这是计算 Fragment Shader 时使用的顶点位置信息。
                float4 position : SV_POSITION;
                //增加法线属性
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;

            };

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            // make fog work
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityStandardBRDF.cginc"

            FragmentData MyVertexProgram(VertexData v){
                FragmentData i;
                // old version: i.position = mul(UNITY_MATRIX_MVP, v.position);
                i.position = UnityObjectToClipPos(v.position);
                i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                i.normal = UnityObjectToWorldNormal(v.normal);

                return i;
            }

            // 在 Fragment Shader 中，程序应该直接以颜色 _MainColor 为结果返回该颜色。
            // 注意这里的返回类型为 float4 ，我们需要指定它的语义为 SV_TARGET 。
            float4 MyFragmentProgram(FragmentData i):SV_Target{
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 lightColor = _LightColor0.rgb;
                float3 diffuse = tex2D(_MainTex, i.uv).rgb * lightColor * DotClamped(lightDir,i.normal);
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * tex2D(_MainTex, i.uv).rgb;
                return float4(diffuse + ambient, 1);
                // return tex2D(_MainTex,i.uv);
            }

            ENDCG
        }
    }
}
