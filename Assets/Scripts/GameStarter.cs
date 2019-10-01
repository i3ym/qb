using qb.Shared;
using qb.Shared.Animations;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public static FieldController Game, AutoGame;
    static IFieldRenderer GameRenderer, AutoGameRenderer;

    [SerializeField]
    public UnityField MainField = null;

    void Start()
    {
        GameRenderer = new FieldRenderer(MainField, new RemoveLinesAnimationSlideDown(), new GameOverFillingAnimation());
        AutoGameRenderer = new FieldRenderer(MainField, new RemoveLinesAnimationSlideDown(), new GameOverFillingAnimation());

        Game = new UnityFieldController(GameRenderer, new PlayerTetrisControlsProvider());
        AutoGame = new UnityFieldController(AutoGameRenderer, new AutoTetrisControlsProvider(AutoGame));

        Game.AfterGameOverAnimation += (obj, e) => Menu.SetMenu<GameOverMenu>();
        AutoGame.AfterGameOverAnimation += (obj, e) => AutoGame.StartGame();
    }
}