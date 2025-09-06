using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VoxelGridPostFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class VoxelSettings
    {
        [Range(0.01f, 5f)] public float voxelSize = 0.15f;
        [Range(2, 64)] public int colorSteps = 8;
        [Range(0f, 0.25f)] public float edgeThickness = 0.05f; // fracción del voxel
        [Range(0f, 1f)] public float edgeDarken = 0.35f;
        public RenderPassEvent injectionPoint = RenderPassEvent.AfterRenderingTransparents;
        public Material material; // shader "Hidden/Post/VoxelGridURP"
    }

    class VoxelGridPass : ScriptableRenderPass
    {
        static readonly string k_Tag = "Voxel Grid Post";
        Material mat;
        VoxelSettings settings;

        RTHandle source;
        RTHandle temp;

        public VoxelGridPass(Material m, VoxelSettings s)
        {
            mat = m;
            settings = s;
            renderPassEvent = s.injectionPoint;
        }

        public void Setup(RTHandle src)
        {
            source = src;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            RenderingUtils.ReAllocateIfNeeded(ref temp, desc, name: "_VoxelGridTemp");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (mat == null) return;

            var cmd = CommandBufferPool.Get(k_Tag);

            mat.SetFloat("_VoxelSize", settings.voxelSize);
            mat.SetFloat("_EdgeThickness", settings.edgeThickness);
            mat.SetFloat("_EdgeDarken", settings.edgeDarken);
            mat.SetInt("_ColorSteps", Mathf.Max(2, settings.colorSteps));

            Blitter.BlitCameraTexture(cmd, source, temp, mat, 0);
            Blitter.BlitCameraTexture(cmd, temp, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (temp != null)
            {
                temp.Release();
                temp = null;
            }
        }
    }

    public VoxelSettings settings = new VoxelSettings();
    VoxelGridPass pass;

    public override void Create()
    {
        if (settings.material == null)
        {
            Shader sh = Shader.Find("Hidden/Post/VoxelGridURP");
            if (sh != null) settings.material = new Material(sh);
        }
        pass = new VoxelGridPass(settings.material, settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material == null) return;
        pass.Setup(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(pass);
    }
}
