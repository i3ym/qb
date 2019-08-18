using System;
using UnityEngine;

public class ControllerButton : MonoBehaviour
{
    public Action OnMouseDownAction = delegate { };
    public Action OnMouseUpAction = delegate { };


    void OnMouseDown() => OnMouseDownAction();
    void OnMouseUp() => OnMouseUpAction();
}