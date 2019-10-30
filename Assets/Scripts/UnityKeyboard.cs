using System;
using qb.Shared.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace qb.Unity
{
    public class UnityKeyboard : MonoBehaviour, IInputManager
    {
        [SerializeField]
        InputAction UpInput = null, DownInput = null, LeftInput = null, RightInput = null,
            BInput = null, AInput = null, PauseInput = null;

        public IButton Up { get; private set; }
        public IButton Down { get; private set; }
        public IButton Left { get; private set; }
        public IButton Right { get; private set; }

        public IButton B { get; private set; }
        public IButton A { get; private set; }
        public IButton Pause { get; private set; }

        void Awake()
        {
            Up = new InputManagerButton(UpInput);
            Down = new InputManagerButton(DownInput);
            Left = new InputManagerButton(LeftInput);
            Right = new InputManagerButton(RightInput);

            B = new InputManagerButton(BInput);
            A = new InputManagerButton(AInput);
            Pause = new InputManagerButton(PauseInput);
        }

        class InputManagerButton : IButton
        {
            public bool Pressed { get; private set; }

            public Action OnPress { get; set; } = delegate { };
            public Action OnRelease { get; set; } = delegate { };

            public InputManagerButton(InputAction input)
            {
                input.started += (_) =>
                {
                    Pressed = true;
                    OnPress();
                };
                input.canceled += (_) =>
                {
                    Pressed = false;
                    OnRelease();
                };

                input.Enable();
            }
        }
    }
}