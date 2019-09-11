using UnityEngine;

public class AutoGame : FieldController
{
    protected override void Awake()
    {
        base.Awake();
        Controls = new AITetrisPlayerControlsProvider(this);
    }

    protected override int GetMaxX() => MaxX;
    protected override int GetMaxY() => MaxY;

    public void SetMax(int x, int y)
    {
        MaxX = x;
        MaxY = y;
    }

    public void StopGame()
    {
        StopAllCoroutines();
        IsPaused = true;

        GameMatrix.Clear();
        CurrentObject.Clear();
        FieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);
    }
    public override void GameOver()
    {
        StopGame();
        RestartGame();
    }
}