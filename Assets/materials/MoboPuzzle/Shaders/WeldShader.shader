Shader "Example/WeldShader"
{
    // The _BaseColor variable is visible in the Material's Inspector, as a field
    // called Base Color. You can use it to select a custom color. This variable
    // has the default value (1, 1, 1, 1).
    Properties
    {
        [MainTexture] _BaseMap("Main Texture", 2D) = "gray"
        _MinEdgeFactor("Min Edge Factor", float) = 1.0
        _MinInsideFactor("Min Inside Factor", float ) = 1.0
        _MaxEdgeFactor("Max Edge Factor", float) = 10.0
        _MaxInsideFactor("Max Inside Factor", float) = 15.0
        _PlayerDistance("Player Distance", float ) = 5.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma hull hull
            #pragma domain domain
            #pragma fragment frag

            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 UV : TEXCOORD;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessellationControlPoint 
            {
                float3 positionWS : INTERNALTESSPOS;
                float2 UV : TEXCOORD;
                float3 normalWS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessellationFactors
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };

            struct Varyings
            {
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float2 UV : TEXCOORD2;
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };


            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float _MinInsideFactor;
                float _MinEdgeFactor;
                float _MaxInsideFactor;
                float _MaxEdgeFactor;
                float _PlayerDistance;
            CBUFFER_END

            TessellationControlPoint vert(Attributes IN)
            {
                TessellationControlPoint OUT;

                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

                VertexPositionInputs vpi = GetVertexPositionInputs(IN.positionOS);
                VertexNormalInputs vni = GetVertexNormalInputs(IN.normal);

                OUT.positionWS = vpi.positionWS;
                OUT.UV = TRANSFORM_TEX(IN.UV, _BaseMap);
                OUT.normalWS = vni.normalWS;

                return OUT;
            }

            TessellationFactors PatchConstantFunction
            (
                InputPatch<TessellationControlPoint, 3> patch
            )
            {
                UNITY_SETUP_INSTANCE_ID(patch[0]);
                TessellationFactors f;
                float dist = clamp(_PlayerDistance, 3.0, 5.0);
                float val = 1.0 - (dist-3.0)/2.0;
                float edgeFactor = lerp(_MinEdgeFactor, _MaxEdgeFactor, val);
                float insideFactor = lerp(_MinInsideFactor, _MaxInsideFactor, val);
                f.edge[0] = edgeFactor;
                f.edge[1] = edgeFactor;
                f.edge[2] = edgeFactor;
                f.inside = insideFactor;
                return f;
            }

            [domain("tri")]
            [outputcontrolpoints(3)]
            [outputtopology("triangle_cw")]
            [patchconstantfunc("PatchConstantFunction")]
            [partitioning("integer")]
            TessellationControlPoint hull
            (
                InputPatch<TessellationControlPoint, 3> patch,
                uint id : SV_OutputControlPointID
            )
            {
                return patch[id];
            }


            #define BARYCENTRIC_INTERPOLATE(fieldName) \
                patch[0].fieldName * barycentricCoordinates.x + \
                patch[1].fieldName * barycentricCoordinates.y + \
                patch[2].fieldName * barycentricCoordinates.z

            [domain("tri")]
            Varyings domain
            (
                TessellationFactors factors,
                OutputPatch<TessellationControlPoint, 3> patch,
                float3 barycentricCoordinates : SV_DomainLocation
            )
            {
                Varyings OUT;

                UNITY_SETUP_INSTANCE_ID(patch[0]);
                UNITY_TRANSFER_INSTANCE_ID(patch[0], OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 positionWS = BARYCENTRIC_INTERPOLATE(positionWS);
                float3 normalWS = BARYCENTRIC_INTERPOLATE(normalWS);
                float2 UV = BARYCENTRIC_INTERPOLATE(UV);

                float3 displacement = 0.1 * normalWS * SAMPLE_TEXTURE2D_LOD(_BaseMap, sampler_BaseMap, UV, 0).g;

                OUT.positionCS = TransformWorldToHClip(positionWS + displacement);
                OUT.normalWS = normalWS;
                OUT.positionWS = positionWS;
                OUT.UV = UV;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Returning the _BaseColor value.
                return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.UV);
            }
            ENDHLSL
        }
    }
}