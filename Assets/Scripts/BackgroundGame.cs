public class BackgroundGame : FieldController
{
    public static new FieldController Instance;

    void Start()
    {
        Instance = this;
        StartGame();
    }

    protected override ITetrisControlsProvider GetProvider() => new AITetrisPlayerControlsProvider(this);
    public override void RestartGame() => StartGame();
    public override void GameOver() => RestartGame();

    protected override void StartGameOverride() { }
}