using UnityEngine.Rendering.Universal;

[System.Serializable]
public class BlurRenderer : ScriptableRendererFeature
{
    BlurPostProcessPass pass;

    public override void Create()
    {
        pass = new BlurPostProcessPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        
        renderer.EnqueuePass(pass);
    }
}
