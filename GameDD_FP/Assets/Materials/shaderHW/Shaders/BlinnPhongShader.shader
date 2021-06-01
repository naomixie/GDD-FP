Shader "Unlit/BlinnPhongShader"
{
    //    镜面反射光是表现一些金属、光滑材质的重要效果，而 Blinn Phong 模型是一个经典的计算镜面反射光
    // 的光照模型。Blinn Phong 模型引入了一个新的向量 H 的概念，它是光入射方向和视线方向之间角平分线的方向,
    // 也叫半矢量（Half Vector）。
    //    通过法线和半矢量之间的点积可以衡量镜面高光的亮度。直观的理解，当点积越大，表示两个向量越接
    // 近，即法线方向越接近光入射方向和视线方向的角平分线，换句话说，视线方向也就越接近镜面反射的
    // 出射光方向，此时镜面反射自然会增大。
    Properties
    {
        // Properties中声明的语法是：变量名（“显示名”，类型） = 默认值
        _MainTex ("Texture", 2D) = "white" {}
        _Gloss ("Gloss", float) = 10 
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
            float _Gloss;

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
                float3 worldPos : TEXCOORD2;

            };

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            // make fog work
            // #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityStandardBRDF.cginc"
            // #include "Math"

            FragmentData MyVertexProgram(VertexData v){
                FragmentData i;
                // old version: i.position = mul(UNITY_MATRIX_MVP, v.position);
                i.position = UnityObjectToClipPos(v.position);
                i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                i.normal = UnityObjectToWorldNormal(v.normal);

                // 将局部坐标系乘上坐标系转换矩阵，计算 FragmentData.worldPos
                i.worldPos = mul(unity_ObjectToWorld, v.position);


                return i;
            }

            // 在 Fragment Shader 中，程序应该直接以颜色 _MainColor 为结果返回该颜色。
            // 注意这里的返回类型为 float4 ，我们需要指定它的语义为 SV_TARGET 。
            float4 MyFragmentProgram(FragmentData i):SV_Target{
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float3 lightColor = _LightColor0.rgb;
                float3 diffuse = tex2D(_MainTex, i.uv).rgb * lightColor * DotClamped(lightDir,i.normal);
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * tex2D(_MainTex, i.uv).rgb;

                // 视线方向 V
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                // 半矢量 H
                float3 halfVector = normalize(lightDir + viewDir);
                // 镜面反射光
                float3 specular = pow(DotClamped(i.normal, halfVector),_Gloss); 

                return float4(diffuse + ambient + specular , 1);
                // return tex2D(_MainTex,i.uv);
            }

            ENDCG
        }
    }
}
