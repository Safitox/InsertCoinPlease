Shader "Post/VoxelGridURP"
{
    Properties { }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "VoxelGrid"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma target 3.0

            // URP includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            TEXTURE2D_X(_BlitTex);           SAMPLER(sampler_BlitTex);      // fuente (color)
            float4 _BlitScaleBias; // usado por Blitter

            float _VoxelSize;          // tamaño del voxel en unidades de mundo
            float _EdgeThickness;      // [0..0.25] fracción de voxel
            float _EdgeDarken;         // [0..1]
            int   _ColorSteps;         // posterización de color

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            // Reconstruct world position from depth
            float3 ReconstructWorldPos(float2 uv)
            {
                #if UNITY_REVERSED_Z
                    float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;
                #else
                    float depth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;
                #endif

                float3 viewPos = ComputeViewSpacePosition(uv, depth, UNITY_MATRIX_I_P);
                float3 worldPos = TransformViewToWorld(viewPos);
                return worldPos;
            }

            // Derive normal from worldpos via screen-space derivatives
            float3 DeriveNormal(float3 wp)
            {
                float3 dpdx = ddx(wp);
                float3 dpdy = ddy(wp);
                float3 n = normalize(cross(dpdx, dpdy));
                return n;
            }

            // Mayor componente -> normal “cuantizada” a ejes (caras de cubo)
            float3 AxisAlignedNormal(float3 n)
            {
                float3 an = abs(n);
                if (an.x > an.y && an.x > an.z)
                    return float3(sign(n.x), 0, 0);
                else if (an.y > an.x && an.y > an.z)
                    return float3(0, sign(n.y), 0);
                else
                    return float3(0, 0, sign(n.z));
            }

            float3 Posterize(float3 c, int steps)
            {
                float s = (steps <= 1) ? 1.0 : (steps - 1);
                return round(c * s) / s;
            }

            float EdgeMask(float3 wp, float voxelSize, float edgeThickness)
            {
                // Posición local dentro del voxel [0..1]
                float3 cell = (wp / voxelSize) - floor(wp / voxelSize);
                float3 distToEdge = min(cell, 1.0 - cell); // distancia a cada cara
                // borde = el mínimo hacia cualquier arista
                float m = min(distToEdge.x, min(distToEdge.y, distToEdge.z));
                // edgeThickness es fracción del voxel: 0=no borde, 0.25=bastante grueso
                float t = edgeThickness;
                // 0 en el borde, 1 en el centro del voxel
                return saturate(smoothstep(0.0, t, m));
            }

            float4 Frag(Varyings i) : SV_Target
            {
                // Fuente de color
                float2 uv = i.uv * _BlitScaleBias.xy + _BlitScaleBias.zw;
                float4 src = SAMPLE_TEXTURE2D_X(_BlitTex, sampler_BlitTex, uv);

                // Reconstrucción de mundo y normales
                float3 wp = ReconstructWorldPos(uv);
                float3 n  = DeriveNormal(wp);

                // “Bloques” fijos en mundo: todo se evalúa con wp y voxelSize
                // Sombreado por cara de cubo (normal a ejes)
                float3 nAxis = AxisAlignedNormal(n);

                // Luz direccional fake para dar sensación de “cara”
                // (ajústalo en C# si quieres exponerlo)
                float3 L = normalize(float3(0.5, 0.8, 0.2));
                float diff = saturate(dot(nAxis, L));
                float shade = 0.35 + 0.65 * diff;

                // Borde de los voxeles para marcar el “canto”
                float emask = EdgeMask(wp, _VoxelSize, _EdgeThickness);
                float edgeDark = lerp(1.0 - _EdgeDarken, 1.0, emask);

                // Posterización de color para look “chunky”
                float3 col = Posterize(src.rgb * shade, _ColorSteps) * edgeDark;

                return float4(col, src.a);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
