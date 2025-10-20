Shader "Custom/WorldCircuitGlow_UnlitTriplanar"
{
    Properties
    {
        _MainTex   ("Base Texture (sRGB ON)", 2D) = "white" {}
        _MaskTex   ("Emission Mask (sRGB OFF)", 2D) = "white" {}
        _Color     ("Base Tint", Color) = (1,1,1,1)

        _Tiling    ("World Tiling (repeats per meter)", Float) = 1.0
        _BlendSharp("Triplanar Blend Sharpness", Range(1,8)) = 4

        _GlowColor ("Glow Color", Color) = (0.0, 1.0, 0.6, 1.0)
        _GlowIntensity ("Glow Intensity (HDR)", Range(0,50)) = 12
        _GlowSpeed ("Glow Speed", Range(0,10)) = 2
        _GlowBias  ("Glow Minimum", Range(0,1)) = 0.25
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "UniversalMaterialType"="Unlit" }

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float3 worldPos    : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
            };

            // Textures
            TEXTURE2D(_MainTex);  SAMPLER(sampler_MainTex);
            TEXTURE2D(_MaskTex);  SAMPLER(sampler_MaskTex);

            // Material params (SRP Batcher-friendly)
            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _GlowColor;
                float  _Tiling;
                float  _BlendSharp;
                float  _GlowIntensity;
                float  _GlowSpeed;
                float  _GlowBias;
            CBUFFER_END

            // Triplanar sampling in world space
            float4 Triplanar(TEXTURE2D_PARAM(tex, samp), float3 worldPos, float3 normalWS, float tiling, float sharp)
            {
                float2 uvX = worldPos.zy * tiling; // project on YZ (X-facing)
                float2 uvY = worldPos.xz * tiling; // project on XZ (Y-facing)
                float2 uvZ = worldPos.xy * tiling; // project on XY (Z-facing)

                float3 n = normalize(normalWS);
                float3 w = pow(abs(n), sharp);
                w /= (w.x + w.y + w.z + 1e-5);

                float4 cx = SAMPLE_TEXTURE2D(tex, samp, uvX);
                float4 cy = SAMPLE_TEXTURE2D(tex, samp, uvY);
                float4 cz = SAMPLE_TEXTURE2D(tex, samp, uvZ);

                return cx * w.x + cy * w.y + cz * w.z;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.worldPos    = TransformObjectToWorld(IN.positionOS).xyz;
                OUT.normalWS    = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Albedo triplanar (anclado al mundo)
                float4 baseCol = Triplanar(TEXTURE2D_ARGS(_MainTex, sampler_MainTex),
                                           IN.worldPos, IN.normalWS, _Tiling, _BlendSharp) * _Color;

                // Máscara (grayscale). IMPORTANTE: importar con sRGB OFF.
                float  mask    = Triplanar(TEXTURE2D_ARGS(_MaskTex, sampler_MaskTex),
                                           IN.worldPos, IN.normalWS, _Tiling, _BlendSharp).r;

                // Pulso (0..1) con mínimo _GlowBias
                float  pulse   = _GlowBias + (1.0 - _GlowBias) * (0.5 + 0.5 * sin(_Time.y * _GlowSpeed));

                // Emisión HDR (Bloom lo toma)
                float3 emission = _GlowColor.rgb * (mask * pulse * _GlowIntensity);

                float3 rgb = baseCol.rgb + emission; // unlit + emisión
                return float4(rgb, 1.0);
            }
            ENDHLSL
        }
    }
}
