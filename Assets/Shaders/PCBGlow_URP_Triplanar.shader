Shader "Custom/PCBGlow_URP_Triplanar"
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
            #pragma shader_feature _ _TRIPLANAR_ON
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

                float _WorldTile;
                float _BlendSharpness;

                        CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                float3 wpos : TEXCOORD1;
                float3 wnorm: TEXCOORD2;
            };

            
            v2f vert (appdata v)
            {
                v2f o;
                float3 wpos = TransformObjectToWorld(v.vertex.xyz);
                o.pos = TransformWorldToHClip(wpos);
                o.uv  = TRANSFORM_TEX(v.uv, _BaseMap);
                o.wpos = wpos;
                o.wnorm = TransformObjectToWorldNormal(v.normal);
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

            
            float3 TriplanarWeights(float3 n)
            {
                float3 an = abs(n);
                float3 w = pow(an, _BlendSharpness);
                return w / max(dot(w, 1.0), 1e-5);
            }

            float4 SampleTriplanar(Texture2D tex, SamplerState smp, float3 wpos, float3 wnorm, float scale)
            {
                float3 w = TriplanarWeights(wnorm);
                float2 uvX = wpos.yz * scale; // projection on X axis (uses YZ)
                float2 uvY = wpos.xz * scale; // projection on Y axis (uses XZ)
                float2 uvZ = wpos.xy * scale; // projection on Z axis (uses XY)
                float4 tx = SAMPLE_TEXTURE2D(tex, smp, uvX);
                float4 ty = SAMPLE_TEXTURE2D(tex, smp, uvY);
                float4 tz = SAMPLE_TEXTURE2D(tex, smp, uvZ);
                return tx * w.x + ty * w.y + tz * w.z;
            }

            float2 TriplanarUVBlend(float3 wpos, float3 wnorm, float scale)
            {
                float3 w = TriplanarWeights(wnorm);
                float2 uvX = wpos.yz * scale;
                float2 uvY = wpos.xz * scale;
                float2 uvZ = wpos.xy * scale;
                return uvX * w.x + uvY * w.y + uvZ * w.z;
            }

            
            half4 frag (v2f i) : SV_Target
            {
                #ifdef _TRIPLANAR_ON
                    float scale = (_Tiling / max(_WorldTile, 1e-5));
                    float2 uvBase  = TriplanarUVBlend(i.wpos, normalize(i.wnorm), scale);
                    float4 baseCol = SampleTriplanar(_BaseMap, sampler_BaseMap, i.wpos, normalize(i.wnorm), scale) * _BaseColor;
                    float  lineMask = SampleTriplanar(_LineMask, sampler_LineMask, i.wpos, normalize(i.wnorm), scale).r;
                #else
                    float2 uvBase  = i.uv * _Tiling;
                    float4 baseCol  = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uvBase) * _BaseColor;
                    float  lineMask = SAMPLE_TEXTURE2D(_LineMask, sampler_LineMask, TRANSFORM_TEX(i.uv, _LineMask)).r;
                #endif

                // Stripe direction & projection
                float a = radians(_AngleDeg);
                float2 dir = float2(cos(a), sin(a));
                float uvProj = dot(uvBase, dir);

                // Time phase
                float phase = _Time.y * _PulseSpeed;

                // Moving band
                float stripe = stripeWave(uvProj, phase, _StripeWidth);

                // Noise sparkles
                float2 uvNoise = (uvBase * _NoiseTiling) + (_Time.y * _BlinkSpeed);
                float n = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, uvNoise).r;
                float spark = step(1.0 - _NoiseAmount, n) * _SparkleIntensity;

                // Emission over lines only
                float glow = (stripe + spark) * saturate(lineMask);
                float3 emissive = _EmissionColor.rgb * glow * _EmissionIntensity;

                float3 finalRGB = baseCol.rgb + emissive;
                return half4(finalRGB, 1);
            }

            ENDHLSL
        }
    }
}
