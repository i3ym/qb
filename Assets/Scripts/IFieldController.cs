using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldController
{
    GameMatrix<bool> GameMatrix { get; }
    GameMatrix<bool> CurrentObject { get; }
    int MaxX { get; }
    int MaxY { get; }
    int CurrentX { get; }
    int CurrentY { get; }
    int CurrentRotation { get; }
    Shapes CurrentShape { get; }
    Shapes NextShape { get; }
}