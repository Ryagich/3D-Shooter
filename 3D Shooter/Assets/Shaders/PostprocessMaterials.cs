using UnityEngine;
using static Unity.VisualScripting.Member;


public class PostprocessMaterials : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    private RenderTexture from, to;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        from = RenderTexture.GetTemporary(source.width, source.height); ;
        to = RenderTexture.GetTemporary(source.width, source.height); ;
        Graphics.Blit(source, from);
        foreach (var mat in _materials)
            Blit(mat);
        Graphics.Blit(from, destination);
        RenderTexture.ReleaseTemporary(from);
        RenderTexture.ReleaseTemporary(to);
    }

    private void Blit(Material mat)
    {
        var passes = mat.shader.passCount;
        for (int i = 0; i < passes; i++)
        {
            Graphics.Blit(from, to, mat, i);
            (to, from) = (from, to);
        }
    }
}