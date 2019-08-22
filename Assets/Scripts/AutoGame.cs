using UnityEngine;

public class AutoGame : FieldController
{
    protected override void Awake()
    {
        base.Awake();
        Controls = new AITetrisPlayerControlsProvider(this);
    }

    protected override int GetMaxX() => ChooseModeMenu.MaxX;
    protected override int GetMaxY() => ChooseModeMenu.MaxY;

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