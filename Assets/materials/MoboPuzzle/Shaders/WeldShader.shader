Shader "Example/WeldShader"
{
    // The _BaseColor variable is visible in the Material's Inspector, as a field
    // called Base Color. You can use it to select a custom color. This variable
    // has the default value (1, 1, 1, 1).
    Properties
    {
        [MainTexture] _BaseMap("Main Texture", 2D) = "black"
        _TexelWidth("Texel width", float) = 0.00125
        _TexelHeight("Texel height", float) = 0.0125
        _ScaleFactor("Scale factor", float) = 1.0
        _MinEdgeFactor("Min Edge Factor", float) = 1.0
        _MinInsideFactor("Min Inside Factor", float ) = 1.0
        _MaxEdgeFactor("Max Edge Factor", float) = 15.0
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
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessellationControlPoint 
            {
                float3 positionWS : INTERNALTESSPOS;
                float2 UV : TEXCOORD;
                float3 normalWS : NORMAL;
                float3 tangentWS : TANGENT;
                float3 bitangentWS : BINORMAL;
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
                float _ScaleFactor;
                float _TexelWidth;
                float _TexelHeight;
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

               OUT.positionWS = TransformObjectToWorld(IN.positionOS);
               OUT.UV = TRANSFORM_TEX(IN.UV, _BaseMap);

               VertexNormalInputs VNI = GetVertexNormalInputs(float3(0.0,1.0,0.0), float4(1.0,0.0,0.0,1.0));

               OUT.normalWS = VNI.normalWS;
               OUT.tangentWS = VNI.tangentWS;
               OUT.bitangentWS = VNI.bitangentWS;

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

               float2 UV = BARYCENTRIC_INTERPOLATE(UV);
               float3 normalWS = patch[0].normalWS;
               float3 tangentWS = patch[0].tangentWS;
               float3 bitangentWS = patch[0].bitangentWS;

               float3 offsetY = _ScaleFactor * normalWS * 0.7;
               float offsetX = _TexelWidth * _ScaleFactor;
               float offsetZ = _TexelHeight * _ScaleFactor;

               float d = 1.0;
               float3 height = offsetY * SAMPLE_TEXTURE2D_LOD(_BaseMap, sampler_BaseMap, UV, 0).g;
               float3 heightx = offsetY * SAMPLE_TEXTURE2D_LOD(_BaseMap, sampler_BaseMap, UV - float2(_TexelWidth*d, 0), 0).g;
               float3 heightz = offsetY * SAMPLE_TEXTURE2D_LOD(_BaseMap, sampler_BaseMap, UV + float2(0,_TexelHeight*d), 0).g;

               float3 pos = BARYCENTRIC_INTERPOLATE(positionWS);
               float3 positionWS =  pos + height;
               float3 positionWSx = pos + tangentWS * offsetX + heightx;
               float3 positionWSz = pos + bitangentWS * offsetZ + heightz;

               float3 tangent = - positionWS + positionWSx; 
               float3 bitangent = - positionWS + positionWSz; 

               float3 newNormal = cross(tangent, bitangent);

               OUT.positionCS = TransformWorldToHClip(positionWS);
               OUT.normalWS = normalize(newNormal);
               OUT.positionWS = positionWS;
               OUT.UV = UV;

               return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
               Light light = GetMainLight();
               float3 lightDir = light.direction;

               float ndotl = dot(IN.normalWS, lightDir);
               half4 defaultColor = half4(half3(0.3, 0.3, 0.3) * light.color * max(ndotl,0.2), 1.0);

               half temp = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.UV).r;
               half4 tempColor = half4(temp,0,0,0.5);

               return tempColor.r > 0.01 ? tempColor : defaultColor;
               //  return tempColor;
               //  return half4(IN.normalWS, 1.0);
               // return half4(IN.UV, 0.0, 1.0);
            }
            ENDHLSL
        }
    }
}