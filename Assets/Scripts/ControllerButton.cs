using System;
using qb.Shared.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace qb.Unity
{
    public class ControllerButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IButton
    {
        public bool Pressed { get; private set; } = false;

        public Action OnPress { get; set; } = delegate { };
        public Action OnRelease { get; set; } = delegate { };

        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
            OnRelease();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            OnPress();
        }
    }
}