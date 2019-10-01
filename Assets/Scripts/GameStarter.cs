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
        var removelinesanim = new RemoveLinesAnimationSlideDown();
        var gameoveranim = new GameOverFillingAnimation();

        GameRenderer = new FieldRenderer(MainField, removelinesanim, gameoveranim);
        AutoGameRenderer = new FieldRenderer(MainField, removelinesanim, gameoveranim);

        Game = new UnityFieldController(GameRenderer, new PlayerTetrisControlsProvider());
        AutoGame = new UnityFieldController(AutoGameRenderer, new AutoTetrisControlsProvider());

        Game.AfterGameOverAnimation += (obj, e) => Menu.SetMenu<GameOverMenu>();
        AutoGame.AfterGameOverAnimation += (obj, e) => AutoGame.StartGame();
    }
}