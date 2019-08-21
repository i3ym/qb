public interface ITetrisControlsProvider
{
    bool Down { get; }
    int Horizontal { get; }

    bool Rotate { get; }
    bool RotateC { get; }
}

public class TetrisPlayerControlsProvider : ITetrisControlsProvider
{
    public bool Down => PlayerControls.Down;
    public int Horizontal => PlayerControls.Horizontal;

    public bool Rotate => PlayerControls.Rotate;
    public bool RotateC => PlayerControls.RotateC;
}

public class AITetrisPlayerControlsProvider : ITetrisControlsProvider
{
    Game Game;

    public AITetrisPlayerControlsProvider(Game game) => Game = game;

    public bool Down => UnityEngine.Random.value >.8f;
    public int Horizontal => UnityEngine.Random.value >.5f ? 1 : -1;

    public bool Rotate => PlayerControls.Rotate;
    public bool RotateC => PlayerControls.RotateC;
}