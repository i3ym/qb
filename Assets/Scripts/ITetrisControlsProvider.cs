using qb.Shared;

public class TetrisPlayerControlsProvider : ITetrisControlsProvider
{
    public bool Down => PlayerControls.Down;
    public int Horizontal => PlayerControls.Horizontal;

    public bool Right => PlayerControls.Right;
    public bool Left => PlayerControls.Left;

    public bool Rotate => PlayerControls.Rotate;
    public bool RotateC => PlayerControls.RotateC;
}

public class AITetrisPlayerControlsProvider : ITetrisControlsProvider
{
    IFieldControllerInfo FieldController;

    public AITetrisPlayerControlsProvider(IFieldControllerInfo fc) => FieldController = fc;

    public bool Down => UnityEngine.Random.value >.8f;
    public int Horizontal => UnityEngine.Random.value >.5f ? 1 : -1;

    public bool Right => Horizontal > 0;
    public bool Left => !Right;

    public bool Rotate => UnityEngine.Random.value >.9f;
    public bool RotateC => false;
}