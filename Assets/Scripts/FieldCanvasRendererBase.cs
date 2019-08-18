using UnityEngine;
using UnityEngine.UI;

public abstract class FieldCanvasRendererBase : FieldRendererBase<RawImage>
{
    protected override void SetMaterial(RawImage t, Material material) => t.color = material.color;
    protected override void SetMaterial(RawImage t, RawImage t2) => t.color = t2.color;

    protected override GameObject GetNewCube(Vector3 pos)
    {
        var go = base.GetNewCube(transform.position);
        (go.transform as RectTransform).anchoredPosition = pos + new Vector3(.5f, .5f);
        return go;
    }
    protected override GameObject GetNewNextCube(Vector3 pos, Transform parent, Material mat)
    {
        var go = base.GetNewNextCube(transform.position, parent, mat);
        (go.transform as RectTransform).anchoredPosition = pos;
        return go;
    }
    protected override GameObject CreateNextObject(Shapes shape)
    {
        var go = base.CreateNextObject(shape);
        go.AddComponent<RectTransform>();

        return go;
    }
}