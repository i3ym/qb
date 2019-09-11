using System.Collections.Generic;
using UnityEngine;

public abstract class FieldRendererBase : MonoBehaviour
{
    [SerializeField]
    protected IFieldRendererExtension[] Extensions = new IFieldRendererExtension[] { };

    protected static Dictionary<Shapes, Color> ShapeColors = new Dictionary<Shapes, Color>();
    protected int MaxX, MaxY;

    static FieldRendererBase()
    {
        ShapeColors.Clear();
        ShapeColors.Add(Shapes.G, Color.blue);
        ShapeColors.Add(Shapes.GR, new Color(1f, .5f, 0f));
        ShapeColors.Add(Shapes.Z, Color.red);
        ShapeColors.Add(Shapes.ZR, Color.green);
        ShapeColors.Add(Shapes.I, new Color(0f, 1f, 1f));
        ShapeColors.Add(Shapes.T, new Color(.5f, 0f, 1f));
        ShapeColors.Add(Shapes.O, Color.yellow);
    }

    public virtual void Initialize()
    {
        foreach (var extension in Extensions) extension.Start();
    }
    protected virtual void Update()
    {
        foreach (var extension in Extensions)
            extension.Update();
    }

    public abstract void SetMaxs(int maxx, int maxy);
    public void UpdateMatrix(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        if (!gameMatrix.Changed) return;

        gameMatrix.Changed = false;
        _UpdateMatrix(gameMatrix, currentObject, currentX, currentY);
    }
    public void UpdateMatrixFast(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        if (!gameMatrix.Changed) return;

        gameMatrix.Changed = false;
        _UpdateMatrixFast(gameMatrix, currentObject, currentX, currentY);
    }
    protected abstract void _UpdateMatrix(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY);
    protected abstract void _UpdateMatrixFast(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY);
    public abstract void UpdateColors(GameMatrix gameMatrix, Shapes currentShape);
    public abstract void ShowNextShape(Shapes shape);

    /// <returns> Duration of animation in seconds </returns> 
    public abstract float RemoveLinesAnimation(List<int> indexs, GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY);
}