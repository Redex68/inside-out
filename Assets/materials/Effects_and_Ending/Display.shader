Shader "Example/WeldShader"
{
    // The _BaseColor variable is visible in the Material's Inspector, as a field
    // called Base Color. You can use it to select a custom color. This variable
    // has the default value (1, 1, 1, 1).
    Properties
    {
        [MainTexture] _BaseMap("Main Texture", 2D) = "black"
        [ResetTime] _ResetTime("Reset Time", float) = 0.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma fragment frag

            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 UV : TEXCOORD;
            };

            struct Varyings
            {
                float2 UV : TEXCOORD2;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float _ResetTime;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT = (Varyings)0;
                
                OUT.UV = IN.UV;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.rgb);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 cameraOutput = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.UV);
                half4 blackness = half4(0.0,0.0,0.0,1.0);
                return lerp(blackness, cameraOutput, clamp(_ResetTime, 0.0, 1.0));
                // return half4(IN.UV.rg, 0.0, 1.0);
            }

            ENDHLSL
        }
    }
}