Shader "Custom/RadialFillSprite_URP_MaterialDriven"
{
    Properties
    {
        [SpriteToTexture]_MainTex ("Sprite (or Texture2D)", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Fill ("Fill Amount", Range(0,1)) = 0
        _StartAngle ("Start Angle (Deg)", Float) = 0
        _Softness ("Edge Softness", Range(0,1)) = 0.02
        _Center ("Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _Clockwise ("Clockwise", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "ForwardUnlit"
            Tags{ "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #ifndef UNITY_PI
                #define UNITY_PI 3.14159265358979323846
            #endif
            #ifndef UNITY_TWO_PI
                #define UNITY_TWO_PI 6.28318530717958647692
            #endif

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float _Fill;
                float _StartAngle;
                float _Softness;
                float4 _Center;
                float _Clockwise;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color;
                return OUT;
            }

            inline float frac01(float x) { return frac(x); }

            float4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                float4 texCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                if (texCol.a <= 0.001) discard;

                float2 c = float2(_Center.x, _Center.y);
                float2 d = uv - c;

                float ang = atan2(d.y, d.x);
                float ang01 = (ang + UNITY_PI) * (1.0 / UNITY_TWO_PI);

                float startFrac = _StartAngle * (1.0 / 360.0);
                ang01 = frac01(ang01 + startFrac);

                if (_Clockwise > 0.5) ang01 = 1.0 - ang01;

                float edge0 = saturate(_Fill - _Softness);
                float edge1 = saturate(_Fill);
                float m = smoothstep(edge0, edge1, ang01);
                float mask = 1.0 - m;

                float4 col = texCol * _Color * IN.color;
                col.rgb *= mask;
                col.a   *= mask;

                if (col.a <= 0.001) discard;
                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
