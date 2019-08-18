using UnityEngine;

public abstract class FieldMeshRendererBase : FieldRendererBase<Renderer>
{
    protected override void SetMaterial(Renderer t, Material material) => t.sharedMaterial = material;
    protected override void SetMaterial(Renderer t, Renderer t2) => t.sharedMaterial = t2.sharedMaterial;
}