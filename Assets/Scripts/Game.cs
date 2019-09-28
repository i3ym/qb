using qb.Shared;

public class Game : FieldController
{
    public static Game Instance;

    public Game(IFieldRenderer renderer) : base(renderer, null)
    {
        Controls = new TetrisPlayerControlsProvider();
        Instance = this;
    }

    public static void UpdateField() => Instance.FieldRenderer.UpdateField();
}