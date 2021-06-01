Shader "Custom/Bump2"
{
    //    纹理的另一种应用是凹凸映射，能够用来修改模型表面的法线，从而在面片不变的情况下展现出较丰
    // 富、有更多细节的图案。这种方法不会真的改变模型的顶点位置，因此在模型的轮廓处能够看出破绽。
    // 凹凸映射有两种方式：
    //  - 高度纹理（height mapping）：用于模拟表面位移，也称作高度映射
    //  - 法线纹理（normal mapping）：用于存储表面法线，也称为法线映射

    Properties
    {
        // Properties中声明的语法是：变量名（“显示名”，类型） = 默认值
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1, 1, 1, 1)
        _BumpMap ("Normal Map", 2D) = "bump" {}         // 使用Unity内置的法线纹理 bump 作为默认值
        _BumpScale ("Bump Scale", Float) = 1.0          // _BumpScale 用于控制凹凸程度，当它的值为0时，该法线纹理不会对光照产生任何影响。
        _Specular ("Specular", Color) = (1, 1, 1, 1)
        _Gloss ("Gloss", Range(8.0, 256)) = 20


    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        // LOD 100

        Pass
        {
            // Tags { "LightMode"="ForwardBase" }
            
            CGPROGRAM

            #include "Lighting.cginc"
            #include "UnityCG.cginc"
            #include "UnityStandardBRDF.cginc"

            //为了在程序中使用变量，我们需要在CGPROGRAM ... ENDCG语块中再次声明相同名字的变量
            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BumpMap;
            float4 _BumpMap_ST;
            float _BumpScale;
            fixed4 _Specular;
            float _Gloss;


            // 由于在凹凸纹理中，shader实际上需要使用两个纹理坐标（实际的纹理和法线纹理），因此，你需
            // 要在片元着色器输入的结构体中存储这两个坐标。
            struct VertexData{
                //我们只需要在 VertexData 中定义一个 float4 类型的变量 position ，并指定它的
                //语义为 POSITION 。这样一来，程序便会让 VertexData.position 被解释为顶点的位置信息。
                float4 position: POSITION;
                float4 uv: TEXCOORD0;
                float2 u2v: TEXCOORD1;

                float3 normal : NORMAL;
                // 需要使用tangent.w分量来决定切线空间中的第三个坐标轴-副切线的方向性
                float4 tangent : TANGENT;
            };

            struct FragmentData{
                //为这个变量指定语义为 SV_POSITION ，表示这是计算 Fragment Shader 时使用的顶点位置信息。
                float4 position : SV_POSITION;
                //增加法线属性
                float4 uv : TEXCOORD0;
                float3 u3v : TEXCOORD5;
                float2 u2v : TEXCOORD6;
                float3 lightDir  : TEXCOORD1;       // 光照方向
                float3 viewDir  : TEXCOORD2;        // 视角方向
                float3 normal : TEXCOORD3;
                float3 worldPos : TEXCOORD4;

            };

            #pragma vertex MyVertexProgram
            #pragma fragment MyFragmentProgram
            #pragma shader_feature USE_NORMAL
            #pragma shader_feature USE_BLINNPHONG
            #pragma shader_feature USE_BUMP


            FragmentData MyVertexProgram(VertexData v){
                FragmentData i;

                #if USE_NORMAL
                    i.position = UnityObjectToClipPos(v.position);
                    i.u3v = UnityObjectToWorldNormal(v.normal);
                    return i;
                #endif

                #if USE_BLINNPHONG
                    i.position = UnityObjectToClipPos(v.position);
                    i.u2v = v.u2v * _MainTex_ST.xy + _MainTex_ST.zw;
                    i.normal = UnityObjectToWorldNormal(v.normal);

                    // 将局部坐标系乘上坐标系转换矩阵，计算 FragmentData.worldPos
                    i.worldPos = mul(unity_ObjectToWorld, v.position);
                    return i;
                #endif
                
                #if USE_BUMP
                    // 将切线方向（x轴）、副切线方向（y轴）和法线方向（z轴）按顺序排列来得到一个
                    // 从模型空间到切线空间的变换矩阵
                    i.position = UnityObjectToClipPos(v.position);
                    i.uv.xy = v.uv.xy *_MainTex_ST.xy + _MainTex_ST.zw;
                    i.uv.zw = v.uv.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                    TANGENT_SPACE_ROTATION;

                    // 利用得到的变换矩阵，将模型空间下的光照和视角方向变换到切线空间中
                    i.lightDir = mul(rotation,ObjSpaceLightDir(v.position)).xyz;
                    i.viewDir = mul(rotation,ObjSpaceViewDir(v.position)).xyz;
                    return i;
                #endif

                    i.position = UnityObjectToClipPos(v.position);
                    i.u3v = UnityObjectToWorldNormal(v.normal);
                    return i;
            }

            // 在 Fragment Shader 中，程序应该直接以颜色 _MainColor 为结果返回该颜色。
            // 注意这里的返回类型为 float4 ，我们需要指定它的语义为 SV_TARGET 。
            float4 MyFragmentProgram(FragmentData i):SV_Target{
                
                #if USE_NORMAL
                    return float4(i.u3v,1);
                #endif

                #if USE_BLINNPHONG
                    float3 lightDir = _WorldSpaceLightPos0.xyz;
                    float3 lightColor = _LightColor0.rgb;
                    float3 diffuse = tex2D(_MainTex, i.u2v).rgb * lightColor * DotClamped(lightDir,i.normal);
                    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * tex2D(_MainTex, i.u2v).rgb;

                    // 视线方向 V
                    float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                    // 半矢量 H
                    float3 halfVector = normalize(lightDir + viewDir);
                    // 镜面反射光
                    float3 specular = pow(DotClamped(i.normal, halfVector),_Gloss); 

                    return float4(diffuse + ambient + specular , 1);
                #endif

                #if USE_BUMP
                    // 并通过像素值和法线值的映射关系得到法线值
                    fixed3 tangentLightDir = normalize(i.lightDir);
                    fixed3 tangentViewDir = normalize(i.viewDir);

                    // 对法线纹理进行采样
                    fixed4 packedNormal = tex2D(_BumpMap, i.uv.zw);
                    fixed3 tangentNormal;

                    tangentNormal = UnpackNormal(packedNormal);
                    // 将处理后的法线向量的x，y分量值乘以 _BumpScale 来控制凹凸程度
                    tangentNormal.xy *= _BumpScale;
                    // 根据法线是单位质量来计算得到z分量
                    tangentNormal.z = sqrt(1.0 - saturate(dot(tangentNormal.xy, tangentNormal.xy)));


                    fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;

                    fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

                    fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(tangentNormal, tangentLightDir));
                    // 半矢量 H
                    fixed3 halfDir = normalize(tangentLightDir + tangentViewDir);

                    // 镜面反射光
                    fixed3 specular = float3(0,0,0);

                    specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(tangentNormal,halfDir)), _Gloss);
                    return fixed4(ambient + diffuse + specular, 1.0);
                #endif

                return float4(i.u3v,1);
            }


            ENDCG
        }

        
    }
    CustomEditor "CustomShaderGUI"
}
