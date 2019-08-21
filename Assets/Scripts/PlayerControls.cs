using UnityEngine;

public class PlayerControls
{
    public static bool Up => Input.GetAxisRaw("Vertical") + (Controller.controller.Up.Pressed ? 1 : 0) > 0;
    public static bool Down => -Input.GetAxisRaw("Vertical") + (Controller.controller.Down.Pressed ? 1 : 0) > 0;
    public static bool Right => Input.GetAxisRaw("Horizontal") + (Controller.controller.Right.Pressed ? 1 : 0) > 0;
    public static bool Left => -Input.GetAxisRaw("Horizontal") + (Controller.controller.Left.Pressed ? 1 : 0) > 0;

    public static int Horizontal => (int) Input.GetAxisRaw("Horizontal") + (Controller.controller.Right.Pressed ? 1 : 0) - (Controller.controller.Left.Pressed ? 1 : 0);
    public static int Vertical => (int) Input.GetAxisRaw("Vertical") + (Controller.controller.Up.Pressed ? 1 : 0) - (Controller.controller.Down.Pressed ? 1 : 0);

    public static bool Rotate => Input.GetKeyDown(KeyCode.E) || Controller.controller.Rotate.Pressed;
    public static bool RotateC => Input.GetKeyDown(KeyCode.Q) || Controller.controller.RotateC.Pressed;
}