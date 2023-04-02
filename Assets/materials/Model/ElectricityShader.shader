Shader "Example/ElectricityShader"
{
    // The _BaseColor variable is visible in the Material's Inspector, as a field
    // called Base Color. You can use it to select a custom color. This variable
    // has the default value (1, 1, 1, 1).
    Properties
    {
        [MainTexture]       _BaseMap("Main Texture", 2D)                = "black"
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
                float3 positionOS : TEXCOORD3;
            };

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);

            bool _PowerOn;
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT = (Varyings)0;
                
                OUT.UV = IN.UV;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.rgb);
                OUT.positionOS = IN.positionOS.rgb;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float speed = 100.0;
                float freq = 10;
                half3 col = half3(0.8, 0.9, 0.1) * 0.4;

                float val = frac(IN.positionOS.b * freq + _Time * speed);

                if(_PowerOn == 1) return half4(lerp(col, half3(1,1,1), val), 1.0);
                else return half4(0.1,0.1,0.1,1);
            }

            ENDHLSL
        }
    }
}