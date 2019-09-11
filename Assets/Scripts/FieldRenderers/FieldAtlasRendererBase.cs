using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldAtlasRendererBase : FieldCanvasRendererBase
{
    static Material Material;
    Dictionary<Shapes, int> Uvs = new Dictionary<Shapes, int>();

    public override void Initialize()
    {
        base.Initialize();

        Uvs.Add(Shapes.I, 0);
        Uvs.Add(Shapes.T, 1);
        Uvs.Add(Shapes.Z, 2);
        Uvs.Add(Shapes.O, 3);
        Uvs.Add(Shapes.ZR, 4);
        Uvs.Add(Shapes.GR, 5);
        Uvs.Add(Shapes.G, 6);
    }

    protected override Material GetMaterial(Shapes shape) => Material;
}