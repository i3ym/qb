public class Game : FieldController
{
    public static Game Instance;

    protected override void Awake()
    {
        base.Awake();
        
        Instance = this;
        Controls = new TetrisPlayerControlsProvider();
    }

    protected override int GetMaxX() => 10;
    protected override int GetMaxY() => 20;

    public static void UpdateField() => Instance.FieldRenderer.UpdateField();
}