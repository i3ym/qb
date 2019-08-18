using UnityEngine;

public abstract class FieldMeshRendererBase : FieldRendererBase<Renderer>
{
    protected override void SetMaterial(Renderer t, Material material) => t.sharedMaterial = material;
    protected override Material GetMaterial(Renderer t) => t.sharedMaterial;
}