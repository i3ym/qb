using qb.Shared;

public class PlayerTetrisControlsProvider : ITetrisControlsProvider
{
    public bool Down => PlayerControls.Down;
    public int Horizontal => PlayerControls.Horizontal;

    public bool Right => PlayerControls.Right;
    public bool Left => PlayerControls.Left;

    public bool Rotate => PlayerControls.Rotate;
    public bool RotateC => PlayerControls.RotateC;
}