public class GameOverMenu : Menu
{
    protected override void Init()
    {
        Buttons[0].OnClick += () =>
        {
            Game.game.RestartGame();
            SetMenu(null);
        };
        Buttons[1].OnClick += () =>
        {
            Game.game.SetDefaults();
            SetMenu<MainMenu>();
        };
    }
}