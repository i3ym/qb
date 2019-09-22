using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static Action OnUp = delegate { };
    public static Action OnDown = delegate { };
    public static Action OnLeft = delegate { };
    public static Action OnRight = delegate { };
    public static Action OnRotate = delegate { };
    public static Action OnRotateC = delegate { };
    public static Action OnSelect = delegate { };
    public static Action OnBack = delegate { };
    public static Action OnPause = delegate { };

    static InputAction UpInput, DownInput, LeftInput, RightInput;
    static InputAction RotateInput, RotateCInput, PauseInput;
    static InputAction SelectInput, BackInput;

    [SerializeField]
    InputAction _UpInput = null, _DownInput = null, _LeftInput = null, _RightInput = null,
        _RotateInput = null, _RotateCInput = null, _SelectInput = null, _BackInput = null, _PauseInput = null;


    void Awake()
    {
        (UpInput = _UpInput).Enable();
        (DownInput = _DownInput).Enable();
        (LeftInput = _LeftInput).Enable();
        (RightInput = _RightInput).Enable();
        (RotateInput = _RotateInput).Enable();
        (RotateCInput = _RotateCInput).Enable();
        (SelectInput = _SelectInput).Enable();
        (BackInput = _BackInput).Enable();
        (PauseInput = _PauseInput).Enable();


        UpInput.started += (_) => OnUp();
        DownInput.performed += (_) => OnDown();
        LeftInput.performed += (_) => OnLeft();
        RightInput.performed += (_) => OnRight();
        RotateInput.performed += (_) => OnRotate();
        RotateCInput.performed += (_) => OnRotateC();
        SelectInput.performed += (_) => OnSelect();
        BackInput.performed += (_) => OnBack();
        PauseInput.performed += (_) => OnPause();


        Controller.controller.Up.OnMouseDownAction += () => OnUp();
        Controller.controller.Down.OnMouseDownAction += () => OnDown();
        Controller.controller.Left.OnMouseDownAction += () => OnLeft();
        Controller.controller.Right.OnMouseDownAction += () => OnRight();
        Controller.controller.Rotate.OnMouseDownAction += () => OnRotate();
        Controller.controller.RotateC.OnMouseDownAction += () => OnRotateC();
        Controller.controller.Select.OnMouseDownAction += () => OnSelect();
        Controller.controller.Back.OnMouseDownAction += () => OnBack();
        Controller.controller.Pause.OnMouseDownAction += () => OnPause();
    }


    public static bool Up => UpInput.phase == InputActionPhase.Started || Controller.controller.Up.Pressed;
    public static bool Down => DownInput.phase == InputActionPhase.Started || Controller.controller.Down.Pressed;
    public static bool Left => LeftInput.phase == InputActionPhase.Started || Controller.controller.Left.Pressed;
    public static bool Right => RightInput.phase == InputActionPhase.Started || Controller.controller.Right.Pressed;

    public static int Horizontal => (Right ? 1 : 0) - (Left ? 1 : 0);
    public static int Vertical => (Up ? 1 : 0) - (Down ? 1 : 0);

    public static bool Rotate => RotateInput.phase == InputActionPhase.Started || Controller.controller.Rotate.Pressed;
    public static bool RotateC => RotateCInput.phase == InputActionPhase.Started || Controller.controller.RotateC.Pressed;
    public static bool Select => SelectInput.phase == InputActionPhase.Started || Controller.controller.Select.Pressed;
    public static bool Back => BackInput.phase == InputActionPhase.Started || Controller.controller.Back.Pressed;
    public static bool Pause => PauseInput.phase == InputActionPhase.Started || Controller.controller.Pause.Pressed;
}