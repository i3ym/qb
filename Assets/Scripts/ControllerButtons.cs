using qb.Shared.Input;
using UnityEngine;

namespace qb.Unity
{
    public class ControllerButtons : ControllerButtonsBase, IControllerButtons
    {
        [SerializeField] ControllerButton _B = null;
        [SerializeField] ControllerButton _A = null;
        [SerializeField] ControllerButton _Pause = null;

        public IButton B => _B;
        public IButton A => _A;
        public IButton Pause => _Pause;
    }
}