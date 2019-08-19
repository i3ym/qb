using UnityEngine;

public class Controls
{
    public static int Up => (int) Input.GetAxisRaw("Vertical") + (Controller.controller.Up.Pressed ? 1 : 0);
    public static int Down => -(int) Input.GetAxisRaw("Vertical") + (Controller.controller.Down.Pressed ? 1 : 0);
    public static int Right => (int) Input.GetAxisRaw("Horizontal") + (Controller.controller.Right.Pressed ? 1 : 0);
    public static int Left => -(int) Input.GetAxisRaw("Horizontal") + (Controller.controller.Left.Pressed ? 1 : 0);
    public static int Horizontal => (int) Input.GetAxisRaw("Horizontal") + (Controller.controller.Right.Pressed ? 1 : 0) - (Controller.controller.Left.Pressed ? 1 : 0);

    public static bool Rotate => Input.GetKeyDown(KeyCode.W) || Controller.controller.Rotate.Pressed;
    public static bool RotateC => Input.GetKeyDown(KeyCode.Q) || Controller.controller.RotateC.Pressed;
}