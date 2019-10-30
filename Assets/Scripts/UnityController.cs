using qb.Shared.Input;
using UnityEngine;

namespace qb.Unity
{
    public class UnityController : MonoBehaviour, IInputManager
    {
        [SerializeField] ControllerArrows Arrows = null;
        [SerializeField] ControllerButtons Buttons = null;

        public IButton Up => Arrows.Up;
        public IButton Down => Arrows.Down;
        public IButton Left => Arrows.Left;
        public IButton Right => Arrows.Right;

        public IButton B => Buttons.B;
        public IButton A => Buttons.A;
        public IButton Pause => Buttons.Pause;
    }
}