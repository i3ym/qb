using System;

public class GameOverMenu : Menu
{
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {
        Buttons[0].OnClick += () =>
        {
            // TODO рекламу и как то подтверждение или нет

            Game.Instance.ContinueGameWithAd();
            SetMenu(null as Menu);
        };
        Buttons[1].OnClick += () =>
        {
            Game.Instance.RestartGame();
            SetMenu(null as Menu);
        };
        Buttons[2].OnClick += () =>
        {
            Game.Instance.SetDefaults();
            SetMenu<MainMenu>();
        };
    }
}