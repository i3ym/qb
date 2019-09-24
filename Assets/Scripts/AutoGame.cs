using UnityEngine;

public class AutoGame : FieldController
{
    public static AutoGame Instance;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
        Controls = new AITetrisPlayerControlsProvider(this);
    }

    protected override int GetMaxX() => 10;
    protected override int GetMaxY() => 20;

    public void SetMax(int x, int y)
    {
        MaxX = x;
        MaxY = y;
    }

    public override void StartGame()
    {
        base.StartGame();
        FieldRenderer.gameObject.SetActive(true);
    }

    public void StopGame()
    {
        IsPaused = true;

        GameMatrix.Clear();
        CurrentObject = new GameMatrix<bool>(CurrentObject.MaxX, CurrentObject.MaxY);
        FieldRenderer.UpdateField();

        FieldRenderer.gameObject.SetActive(false);
    }
    public override void GameOver()
    {
        StopGame();
        RestartGame();
    }

    public static void UpdateField() => Instance.FieldRenderer.UpdateField();
}