using qb.Shared;
using UnityEngine;

public class AutoGame : FieldController<FieldObjectRenderer>
{
    public static AutoGame Instance;

    public AutoGame(FieldObjectRenderer renderer) : base(renderer, null)
    {
        Controls = new AITetrisPlayerControlsProvider(this);
        Instance = this;
    }

    public override void StartGame()
    {
        base.StartGame();
        FieldRenderer.FieldRendererInfo.gameObject.SetActive(true);
    }

    public void StopGame()
    {
        IsPaused = true;

        GameMatrix.Clear();
        CurrentObject = new GameMatrix<bool>(CurrentObject.MaxX, CurrentObject.MaxY);
        FieldRenderer.UpdateField();

        FieldRenderer.FieldRendererInfo.gameObject.SetActive(false);
    }
    public override void GameOver()
    {
        StopGame();
        StartGame();
    }

    public static void UpdateField() => Instance.FieldRenderer.UpdateField();
}