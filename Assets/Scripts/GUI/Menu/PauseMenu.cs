using System;

public class PauseMenu : Menu
{
    protected override Type PreviousMenuType => null;

    protected override void Init()
    {
        PlayerControls.OnPause += () =>
        {
            if (ActiveController == null) SetMenu(this);
        };

        Buttons[0].OnClick += () =>
        {
            GameStarter.Game.IsPaused = false;
            SetMenu(null as Menu);
        };
        Buttons[1].OnClick += () =>
        {
            GameStarter.Game.IsPaused = false;
            GameStarter.Game.StartGame();
            SetMenu(null as Menu);
        };
        Buttons[2].OnClick += () => SetMenu<MainMenu>();
    }

    protected override void OnShow() => GameStarter.Game.IsPaused = true;
    protected override void OnGoBack() => GameStarter.Game.IsPaused = false;
}