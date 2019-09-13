using System;
using UnityEngine;

public class MainMenu : Menu
{
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {
        Buttons[0].OnClick += () => SetMenu<ChooseModeMenu>();
        Buttons[1].OnClick += () => SetMenu<ChooseSkinMenu>();
        Buttons[2].OnClick += () => Application.Quit();

        SetMenu(this);
    }
}