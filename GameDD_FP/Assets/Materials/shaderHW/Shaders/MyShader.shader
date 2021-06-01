Shader "Unlit/MyShader"
{
    Properties
    {
        // Properties中声明的语法是：变量名（“显示名”，类型） = 默认值
        // _MainTex ("Texture", 2D) = "white" {}
        _MainColor("Main Color",Color) = (1,1,1,1)
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        // LOD 100

        Pass
        {
            CGPROGRAM
            //为了在程序中使用变量，我们需要在CGPROGRAM ... ENDCG语块中再次声明相同名字的变量
            float4 _MainColor;

            struct VertexData{
                //我们只需要在 VertexData 中定义一个 float4 类型的变量 position ，并指定它的
                //语义为 POSITION 。这样一来，程序便会让 VertexData.position 被解释为顶点的位置信息。
                float4 position: POSITION;
                // 增加法线属性
                float3 normal: NORMAL;
            };

            struct FragmentData{
                //为这个变量指定语义为 SV_POSITION ，表示这是计算 Fragment Shader 时使用的顶点位置信息。
                float4 position : SV_POSITION;
                //增加法线属性
                float3 normal : TEXCOORD0;
            };

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            // make fog work
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"

            // struct appdata
            // {
            //     float4 vertex : POSITION;
            //     float2 uv : TEXCOORD0;
            // };

            // struct v2f
            // {
            //     float2 uv : TEXCOORD0;
            //     UNITY_FOG_COORDS(1)
            //     float4 vertex : SV_POSITION;
            // };

            FragmentData MyVertexProgram(VertexData v){
                FragmentData i;
                // old version: i.position = mul(UNITY_MATRIX_MVP, v.position);
                i.position = UnityObjectToClipPos(v.position);
                i.normal = UnityObjectToWorldNormal(v.normal);
                return i;
            }

            // 在 Fragment Shader 中，程序应该直接以颜色 _MainColor 为结果返回该颜色。
            // 注意这里的返回类型为 float4 ，我们需要指定它的语义为 SV_TARGET 。
            float4 MyFragmentProgram(FragmentData i):SV_Target{
                return float4(i.normal,1);
            }

            // sampler2D _MainTex;
            // float4 _MainTex_ST;

            // v2f vert (appdata v)
            // {
            //     v2f o;
            //     o.vertex = UnityObjectToClipPos(v.vertex);
            //     o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            //     UNITY_TRANSFER_FOG(o,o.vertex);
            //     return o;
            // }

            // fixed4 frag (v2f i) : SV_Target
            // {
            //     // sample the texture
            //     fixed4 col = tex2D(_MainTex, i.uv);
            //     // apply fog
            //     UNITY_APPLY_FOG(i.fogCoord, col);
            //     return col;
            // }

            // void MyVertexProgram() {}
            // void MyFragmentProgram() {}
            ENDCG
        }
    }
}
