Shader "Custom/PCBGlow_URP"
{
    Properties
    {
        _BaseMap("Base Map (RGB)", 2D) = "white" {}
        _LineMask("Line Mask (R=lines)", 2D) = "white" {}
        _NoiseTex("Noise (optional)", 2D) = "gray" {}
        _BaseColor("Base Color", Color) = (1,1,1,1)

        _EmissionColor("Emission Color", Color) = (0.2,1,0.5,1)
        _EmissionIntensity("Emission Intensity", Range(0,5)) = 1

        _Tiling("UV Tiling", Float) = 1
        _AngleDeg("Stripe Angle (deg)", Range(0,360)) = 45
        _PulseSpeed("Pulse Speed", Range(0,3)) = 0.6
        _StripeWidth("Stripe Width", Range(0.01,0.5)) = 0.12

        _NoiseTiling("Noise Tiling", Float) = 3
        _BlinkSpeed("Blink Speed", Range(0,5)) = 1.2
        _NoiseAmount("Noise Amount", Range(0,1)) = 0.85
        _SparkleIntensity("Sparkle Intensity", Range(0,2)) = 0.6
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 100

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);       SAMPLER(sampler_BaseMap);
            TEXTURE2D(_LineMask);      SAMPLER(sampler_LineMask);
            TEXTURE2D(_NoiseTex);      SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _LineMask_ST;
                float4 _NoiseTex_ST;
                float4 _BaseColor;
                float4 _EmissionColor;

                float _EmissionIntensity;
                float _Tiling;
                float _AngleDeg;
                float _PulseSpeed;
                float _StripeWidth;

                float _NoiseTiling;
                float _BlinkSpeed;
                float _NoiseAmount;
                float _SparkleIntensity;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv  = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            float stripeWave(float uvProj, float phase, float width)
            {
                // banda suave que recorre en bucle
                float f = frac(uvProj - phase);      // 0..1
                float d = 0.5 - abs(f - 0.5);        // pico en 0.5
                // d en [0..0.5]; normalizamos alrededor de width
                return smoothstep(width*0.2, width, d);
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 uvBase  = i.uv * _Tiling;

                // Texturas
                float4 baseCol  = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uvBase) * _BaseColor;
                float  lineMask = SAMPLE_TEXTURE2D(_LineMask, sampler_LineMask, TRANSFORM_TEX(i.uv, _LineMask)).r;

                // Dirección del barrido
                float a = radians(_AngleDeg);
                float2 dir = float2(cos(a), sin(a));
                float uvProj = dot(uvBase, dir);

                // Fase temporal
                float phase = _Time.y * _PulseSpeed;

                // Banda que viaja
                float stripe = stripeWave(uvProj, phase, _StripeWidth);

                // Chisporroteos (opcional)
                float2 uvNoise = (uvBase * _NoiseTiling) + (_Time.y * _BlinkSpeed);
                float n = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, uvNoise).r;
                float spark = step(1.0 - _NoiseAmount, n) * _SparkleIntensity;

                // Emisión final solo sobre las líneas
                float glow = (stripe + spark) * saturate(lineMask);
                float3 emissive = _EmissionColor.rgb * glow * _EmissionIntensity;

                // Color final (emisión añadida)
                float3 finalRGB = baseCol.rgb + emissive;
                return half4(finalRGB, 1);
            }
            ENDHLSL
        }
    }
}
