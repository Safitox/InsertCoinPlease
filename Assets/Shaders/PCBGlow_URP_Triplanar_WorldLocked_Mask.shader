Shader "Custom/PCBGlow_URP_Triplanar_WorldLocked+Mask"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)

        _TileSize("World Tile Size (meters per repeat)", Float) = 1.0
        _BlendSharpness("Triplanar Blend Sharpness", Range(1,12)) = 5.0

        [Header(Emissive Stripes)]
        _StripeEnabled("Enable Emissive Stripes", Float) = 1.0
        _StripePeriod("Stripe Period (meters)", Float) = 0.25
        _StripeWidth("Stripe Width (0-1 of period)", Range(0.01, 0.9)) = 0.25
        _StripeAngleY("Stripe Angle around Y (deg)", Range(0,360)) = 0.0
        _StripeSpeed("Stripe Scroll Speed", Float) = 0.0
        _StripeIntensity("Stripe Intensity", Range(0,10)) = 2.0
        _EmissionColor("Emission Color", Color) = (0.2, 1.0, 0.6, 1.0)

        [Header(Mask)]
        _LineMask("Line Mask (R)", 2D) = "white" {}
        _MaskTileSize("Mask World Tile Size (m)", Float) = 1.0
        _MaskUseUVs("Mask: Use Mesh UVs (off=Triplanar)", Float) = 0.0
        _MaskInvert("Mask Invert", Float) = 0.0
        _MaskAffectsEmission("Mask Affects Emission", Float) = 1.0
        _MaskAffectsBase("Mask Affects Base", Float) = 0.0

        [Header(Surface)]
        _Smoothness("Smoothness", Range(0,1)) = 0.0
        _Metallic("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags{ "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_LineMask); SAMPLER(sampler_LineMask);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _TileSize;
                float _BlendSharpness;

                float _StripeEnabled;
                float _StripePeriod;
                float _StripeWidth;
                float _StripeAngleY;
                float _StripeSpeed;
                float _StripeIntensity;
                float4 _EmissionColor;

                float _Smoothness;
                float _Metallic;

                float4 _LineMask_ST;
                float _MaskTileSize;
                float _MaskUseUVs;
                float _MaskInvert;
                float _MaskAffectsEmission;
                float _MaskAffectsBase;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float2 uv           : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionWS = positionWS;
                OUT.positionCS = TransformWorldToHClip(positionWS);

                // Proper world-space normal (handles non-uniform scale)
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            // Helper: triplanar sampling of a single texture using world-space position
            float4 SampleTriplanar(TEXTURE2D_PARAM(tex, samp), float3 worldPos, float3 worldNormal, float tileSize, float blendSharpness)
            {
                // Avoid divide by zero
                tileSize = max(tileSize, 1e-5);

                // World-projected UVs (meters -> repeats)
                float2 uvX = worldPos.zy / tileSize; // YZ plane for X-facing
                float2 uvY = worldPos.xz / tileSize; // XZ plane for Y-facing
                float2 uvZ = worldPos.xy / tileSize; // XY plane for Z-facing

                float3 n = normalize(abs(worldNormal));
                n = pow(n, blendSharpness);
                float denom = max(n.x + n.y + n.z, 1e-5);
                float3 w = n / denom;

                float4 cx = SAMPLE_TEXTURE2D(tex, samp, uvX);
                float4 cy = SAMPLE_TEXTURE2D(tex, samp, uvY);
                float4 cz = SAMPLE_TEXTURE2D(tex, samp, uvZ);

                return cx * w.x + cy * w.y + cz * w.z;
            }

            // World-locked emissive stripes
            float StripeMask(float3 worldPos, float period, float width, float angleYDeg, float time, float speed)
            {
                if (period <= 1e-5) return 0.0;

                // Direction in XZ plane by angle around Y
                float ang = radians(angleYDeg);
                float2 dir = float2(cos(ang), sin(ang));

                // Project world position onto that direction and scroll by time*speed
                float coord = dot(worldPos.xz, dir) + time * speed;

                float t = frac(coord / period);
                // inside narrow band defined by width
                float halfW = saturate(width * 0.5);
                // Soft edges around band center
                float edge = 0.5 * (1.0 - halfW);
                float m = smoothstep(edge, edge + 0.01, t) * (1.0 - smoothstep(1.0 - edge - 0.01, 1.0 - edge, t));
                return m;
            }

            float SampleMask(float3 worldPos, float3 worldNormal, float2 uv, float maskTileSize, float blendSharpness)
            {
                // Option: use mesh UVs (transformed) or triplanar world-locked
                if (_MaskUseUVs > 0.5)
                {
                    float2 uvm = TRANSFORM_TEX(uv, _LineMask);
                    return SAMPLE_TEXTURE2D(_LineMask, sampler_LineMask, uvm).r;
                }
                else
                {
                    float4 m = SampleTriplanar(TEXTURE2D_ARGS(_LineMask, sampler_LineMask), worldPos, worldNormal, maskTileSize, blendSharpness);
                    return m.r;
                }
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                float3 worldPos = IN.positionWS;
                float3 worldNormal = normalize(IN.normalWS);

                // Base color by triplanar sample (world-locked)
                float4 baseSample = SampleTriplanar(TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap), worldPos, worldNormal, _TileSize, _BlendSharpness);
                float3 baseColor = baseSample.rgb * _BaseColor.rgb;

                // Mask (R)
                float maskR = SampleMask(worldPos, worldNormal, IN.uv, max(_MaskTileSize, 1e-5), _BlendSharpness);
                maskR = saturate(_MaskInvert > 0.5 ? 1.0 - maskR : maskR);

                // Optional: apply mask to base (multiply)
                if (_MaskAffectsBase > 0.5)
                {
                    baseColor *= maskR;
                }

                // Simple main light lambert
                Light light = GetMainLight();
                float NdotL = saturate(dot(worldNormal, light.direction));
                float3 litColor = baseColor * (0.2 + 0.8 * NdotL);

                // Emissive stripes (world-locked)
                float t = _Time.y;
                float stripe = (_StripeEnabled > 0.5) ? StripeMask(worldPos, _StripePeriod, _StripeWidth, _StripeAngleY, t, _StripeSpeed) : 0.0;

                // Mask emission if enabled
                if (_MaskAffectsEmission > 0.5)
                {
                    stripe *= maskR;
                }

                float3 emission = stripe * _StripeIntensity * _EmissionColor.rgb;

                float3 color = litColor + emission;

                return half4(color, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack Off
}