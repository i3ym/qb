using qb.Shared.Input;
using UnityEngine;

namespace qb.Unity
{
    public class ControllerArrows : ControllerButtonsBase, IControllerArrows
    {
        [SerializeField] ControllerButton _Up = null;
        [SerializeField] ControllerButton _Down = null;
        [SerializeField] ControllerButton _Left = null;
        [SerializeField] ControllerButton _Right = null;

        public IButton Up => _Up;
        public IButton Down => _Down;
        public IButton Left => _Left;
        public IButton Right => _Right;
    }
}