Shader "Example/RenderScreenGPUShader"
{
    // The _BaseColor variable is visible in the Material's Inspector, as a field
    // called Base Color. You can use it to select a custom color. This variable
    // has the default value (1, 1, 1, 1).
    Properties
    {
        [MainTexture]       _BaseMap("Main Texture", 2D)                = "black"
        [TextureRes]        _TextureRes("Texture Resolution", int)      = 100
        [RasterizeCount]    _RasterizeCount("Rasterize Count", int)     = 0
        [RasterComplete]    _RasterComplete("Raster Complete", int)    = 0
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
                int _TextureRes;
                int _RasterizeCount;
                int _RasterComplete;
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
                half4 cameraOutput = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, (IN.UV + float2(0.0,0.1)) / float2(1.0,0.8));
                half4 blackness = half4(0.0,0.0,0.0,1.0);

                if(_RasterComplete) return cameraOutput;
                
                float x = IN.UV.x * _TextureRes;
                float y = IN.UV.y * _TextureRes;

                float dist = min(abs(round(x)-x), abs(round(y)-y));

                int count = (int)y * _TextureRes + (int)x;
                count = clamp(count, 0, _TextureRes * _TextureRes);


                float perc = 1 - 1.0 * count / _TextureRes / _TextureRes;

                return lerp
                (
                    count <= _RasterizeCount ? cameraOutput : blackness, 
                    blackness,
                    clamp(sqrt(perc) * sqrt(1.0 - dist),0.0,1.0)
                );
            }

            ENDHLSL
        }
    }
}