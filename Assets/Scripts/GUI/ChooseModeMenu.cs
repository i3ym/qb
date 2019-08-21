public class ChooseModeMenu : Menu
{
    protected override void Init()
    {
        Buttons[0].OnSelect += () =>
        {
            Game.BlindRendering = false;
            Game.UpdateMatrix();
        };
        Buttons[1].OnSelect += () =>
        {
            Game.BlindRendering = true;
            Game.UpdateMatrix();
        };

        Buttons[0].OnClick += () =>
        {
            Game.BlindRendering = false;
            Game.IsAutomatic = false;
            Game.Instance.RestartGame();
            SetMenu(null);
        };
        Buttons[1].OnClick += () =>
        {
            Game.BlindRendering = true;
            Game.IsAutomatic = false;
            Game.Instance.RestartGame();
            SetMenu(null);
        };

        Buttons[2].OnClick += () => SetMenu<MainMenu>();
    }
}