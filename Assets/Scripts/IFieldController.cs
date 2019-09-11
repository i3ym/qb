using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldController
{
    GameMatrix GameMatrix { get; }
    GameMatrix CurrentObject { get; }
    int CurrentX { get; }
    int CurrentY { get; }
    int CurrentRotation { get; }
    Shapes CurrentShape { get; }
    Shapes NextShape { get; }
}