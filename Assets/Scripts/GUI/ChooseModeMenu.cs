public class ChooseModeMenu : Menu
{
    protected override void Init()
    {
        Buttons[0].OnSelect += () =>
        {
            Game.BlindRendering = false;
        };

        Buttons[0].OnSelect += () =>
        {
            Game.BlindRendering = true;
        };

        Buttons[0].OnClick += () =>
        {
            Game.BlindRendering = false;
            Game.game.StartGame();
            SetMenu(null);
        };
        Buttons[1].OnClick += () =>
        {
            Game.BlindRendering = true;
            Game.game.StartGame();
            SetMenu(null);
        };

        Buttons[2].OnClick += () => SetMenu<MainMenu>();
    }
}