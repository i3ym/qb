using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool Pressed { get; private set; } = false;

    public Action OnMouseDownAction = delegate { };
    public Action OnMouseUpAction = delegate { };


    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        OnMouseUpAction();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        OnMouseDownAction();
    }
}