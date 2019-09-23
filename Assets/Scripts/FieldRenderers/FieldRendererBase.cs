using System.Collections.Generic;
using UnityEngine;

public abstract class FieldRendererBase : MonoBehaviour, IFieldRenderer
{
    [SerializeField]
    protected IFieldRendererExtension[] Extensions = new IFieldRendererExtension[] { };

    public IFieldController Controller { get; protected set; }

    int LastX, LastY, LastRotation;

    protected GameMatrix<bool> GameMatrix => Controller.GameMatrix;
    protected GameMatrix<bool> CurrentObject => Controller.CurrentObject;
    protected int MaxX => Controller.MaxX;
    protected int MaxY => Controller.MaxY;
    protected int CurrentX => Controller.CurrentX;
    protected int CurrentY => Controller.CurrentY;
    protected int CurrentRotation => Controller.CurrentRotation;
    protected Shapes CurrentShape => Controller.CurrentShape;
    protected Shapes NextShape => Controller.NextShape;

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

    public abstract IEnumerator<bool> RemoveLinesAnimation(int[] lines);
    public abstract IEnumerator<bool> GameOverAnimation();
}