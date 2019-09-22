using System.Collections.Generic;
using UnityEngine;

public abstract class FieldRendererBase : MonoBehaviour, IFieldRenderer
{
    [SerializeField]
    protected IFieldRendererExtension[] Extensions = new IFieldRendererExtension[] { };

    public IFieldController Controller { get; protected set; }

    int LastX, LastY, LastRotation;

    protected GameMatrix GameMatrix => Controller.GameMatrix;
    protected GameMatrix CurrentObject => Controller.CurrentObject;
    protected int MaxX => Controller.MaxX;
    protected int MaxY => Controller.MaxY;
    protected int CurrentX => Controller.CurrentX;
    protected int CurrentY => Controller.CurrentY;
    protected int CurrentRotation => Controller.CurrentRotation;
    protected Shapes CurrentShape => Controller.CurrentShape;
    protected Shapes NextShape => Controller.NextShape;

    protected static Dictionary<Shapes, Color> ShapeColors = new Dictionary<Shapes, Color>();

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

    public virtual void Initialize(IFieldController controller)
    {
        Controller = controller;
        foreach (var extension in Extensions) extension.Start();
    }
    protected virtual void Update()
    {
        foreach (var extension in Extensions)
            extension.Update();
    }

    public abstract void CreateField();
    public void UpdateField()
    {
        if (!GameMatrix.Changed && LastX == CurrentX && LastY == CurrentY && LastRotation == CurrentRotation) return;

        LastX = CurrentX;
        LastY = CurrentY;
        LastRotation = CurrentRotation;
        GameMatrix.Changed = false;
        _UpdateMatrix();
    }
    public void UpdateFieldFast()
    {
        if (!GameMatrix.Changed && LastX == CurrentX && LastY == CurrentY && LastRotation == CurrentRotation) return;

        LastX = CurrentX;
        LastY = CurrentY;
        LastRotation = CurrentRotation;
        GameMatrix.Changed = false;
        _UpdateMatrixFast();
    }
    protected abstract void _UpdateMatrix();
    protected abstract void _UpdateMatrixFast();
    public abstract void UpdateColors();
    public abstract void ShowNextShape();

    public abstract IEnumerator<bool> RemoveLinesAnimation(int[] indexs);
}