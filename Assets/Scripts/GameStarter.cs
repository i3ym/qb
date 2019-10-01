using qb.Shared;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public static FieldController Game, AutoGame;
    static IFieldRenderer GameRenderer, AutoGameRenderer;

    [SerializeField]
    public UnityField MainField = null;

    void Start()
    {
        GameRenderer = new FieldRenderer(MainField);
        AutoGameRenderer = new FieldRenderer(MainField);

        Game = new UnityFieldController(GameRenderer, new PlayerTetrisControlsProvider());
        AutoGame = new UnityFieldController(AutoGameRenderer, new AutoTetrisControlsProvider(AutoGame));

        AutoGame.OnDeath += (obj, e) => AutoGame.StartGame();
    }
}