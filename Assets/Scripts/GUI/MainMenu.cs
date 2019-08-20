using UnityEngine;

public class MainMenu : Menu
{
    protected override void Init()
    {
        Buttons[0].OnClick += () => SetMenu<ChooseModeMenu>();
        Buttons[1].OnClick += () => Application.Quit();

        SetMenu(this);
    }
}