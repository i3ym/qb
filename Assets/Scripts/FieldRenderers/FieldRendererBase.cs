using System.Collections.Generic;
using UnityEngine;

public abstract class FieldRendererBase : MonoBehaviour
{
    [SerializeField]
    protected IFieldRendererExtension[] Extensions = new IFieldRendererExtension[] { };

    protected Dictionary<Shapes, Color> ShapeColors = new Dictionary<Shapes, Color>();
    protected int MaxX, MaxY;

    GameMatrix UpdateGameMatrix, UpdateCurrentObject;
    int UpdateCurrentX, UpdateCurrentY;
    bool DoUpdateMatrix = false, DoUpdateMatrixFast = false;

    public virtual void Initialize()
    {
        foreach (var extension in Extensions) extension.Start();

        ShapeColors.Add(Shapes.G, Color.blue);
        ShapeColors.Add(Shapes.GR, new Color(1f, .5f, 0f));
        ShapeColors.Add(Shapes.Z, Color.red);
        ShapeColors.Add(Shapes.ZR, Color.green);
        ShapeColors.Add(Shapes.I, new Color(0f, 1f, 1f));
        ShapeColors.Add(Shapes.T, new Color(.5f, 0f, 1f));
        ShapeColors.Add(Shapes.O, Color.yellow);
    }
    protected virtual void Update()
    {
        foreach (var extension in Extensions)
            extension.Update();
    }
    void LateUpdate()
    {
        if (DoUpdateMatrix)
        {
            _UpdateMatrix(UpdateGameMatrix, UpdateCurrentObject, UpdateCurrentX, UpdateCurrentY);
            DoUpdateMatrix = false;
            DoUpdateMatrixFast = false;
        }
        else if (DoUpdateMatrixFast)
        {
            _UpdateMatrixFast(UpdateGameMatrix, UpdateCurrentObject, UpdateCurrentX, UpdateCurrentY);
            DoUpdateMatrixFast = false;
        }
    }


    public abstract void SetMaxs(int maxx, int maxy);
    public void UpdateMatrix(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        UpdateGameMatrix = gameMatrix;
        UpdateCurrentObject = currentObject;
        UpdateCurrentX = currentX;
        UpdateCurrentY = currentY;

        DoUpdateMatrix = true;
    }
    public void UpdateMatrixFast(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        UpdateGameMatrix = gameMatrix;
        UpdateCurrentObject = currentObject;
        UpdateCurrentX = currentX;
        UpdateCurrentY = currentY;

        DoUpdateMatrixFast = true;
    }
    protected abstract void _UpdateMatrix(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY);
    protected abstract void _UpdateMatrixFast(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY);
    public abstract void UpdateColors(GameMatrix gameMatrix, Shapes currentShape);
    public abstract void ShowNextShape(Shapes shape);

    /// <returns> Duration of animation in seconds </returns> 
    public abstract float RemoveLinesAnimation(List<int> y, GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY);
}