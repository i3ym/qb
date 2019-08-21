public class GameOverMenu : Menu
{
    protected override void Init()
    {
        Buttons[0].OnClick += () =>
        {
            Game.Instance.RestartGame();
            SetMenu(null);
        };
        Buttons[1].OnClick += () =>
        {
            Game.Instance.SetDefaults();
            Game.IsAutomatic = true;
            Game.Instance.StartGame();
            SetMenu<MainMenu>();
        };
    }
}