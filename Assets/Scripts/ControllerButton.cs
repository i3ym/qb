using System;
using UnityEngine;

public class ControllerButton : MonoBehaviour
{
    public bool Pressed { get; private set; } = false;

    public Action OnMouseDownAction = delegate { };
    public Action OnMouseUpAction = delegate { };

    void OnMouseDown()
    {
        Pressed = true;
        OnMouseDownAction();
    }
    void OnMouseUp()
    {
        Pressed = false;
        OnMouseUpAction();
    }
}