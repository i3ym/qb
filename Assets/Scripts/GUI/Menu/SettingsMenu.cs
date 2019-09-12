using System;

public class SettingsMenu : Menu
{
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {
        Buttons[0].OnClick += () => SetMenu<SettingsGraphicsMenu>();
        Buttons[1].OnClick += () => SetMenu<ChooseSkinMenu>();
    }
}