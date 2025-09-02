Shader "Custom/RadialFillSprite_URP"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
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

            float frac01(float x) { return x - floor(x); }

            half4 frag (Varyings IN) : SV_Target
            {
                // Sample sprite
                float2 uv = IN.uv;
                half4 texCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                // Early discard based on alpha to avoid halo at edges
                if (texCol.a <= 0.001) discard;

                // Centered UV for angle computation
                float2 c = float2(_Center.x, _Center.y);
                float2 d = uv - c;

                // atan2 in radians [-PI, PI] -> normalize to [0,1]
                float ang = atan2(d.y, d.x);
                const float PI = 3.14159265359;
                float ang01 = (ang + PI) / (2.0 * PI);

                // Start angle offset (degrees -> 0..1)
                float startFrac = _StartAngle / 360.0;
                ang01 = frac01(ang01 + startFrac);

                // Optional clockwise inversion
                if (_Clockwise > 0.5) ang01 = 1.0 - ang01;

                // Smooth radial mask from 1 (filled) to 0 (empty) around Fill
                float edge0 = saturate(_Fill - _Softness);
                float edge1 = saturate(_Fill);
                float m = smoothstep(edge0, edge1, ang01);
                float mask = 1.0 - m;

                // Compose final color
                half4 col = texCol * _Color * IN.color;
                col.rgb *= mask;
                col.a *= mask;

                // Kill fully transparent
                if (col.a <= 0.001) discard;
                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
