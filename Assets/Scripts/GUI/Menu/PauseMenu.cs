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
            Game.Instance.IsPaused = false;
            SetMenu(null as Menu);
        };
        Buttons[1].OnClick += () => SetMenu<MainMenu>();
    }

    protected override void OnShow() => Game.Instance.IsPaused = true;
    protected override void OnGoBack() => Game.Instance.IsPaused = false;
}