Shader "Test/BlackAndWhite"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragma frag
            #include "Packages/com.unity.render-pipeline.universal/ShaderLibrary/Core.hlsl"
        
            float4 _BaseColor;

            struct appdata
            {
                float4 positionOS : POSITION;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o = {v2f}0; 

                o.positionCS = TransformObjectToHClip(v.positionOs.xyz);

                return o;
            }

            float frag(v2f i) : SV_TARGET
            {
                return _BaseColor;
            }

            ENDHLSL
        }
    }
}