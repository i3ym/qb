using System;
using TMPro;
using UnityEngine;

public class ChooseLvlMenu : Menu
{
    protected override Type PreviousMenuType => typeof(ChooseModeMenu);

    [SerializeField]
    TextMeshProUGUI LevelText = null;

    protected override void Init()
    {
        LevelText.text = "0";

        Buttons[0].OnClick += () =>
        {
            GameStarter.Game.StartLevel++;
            LevelText.text = GameStarter.Game.StartLevel.ToString();
        };
        Buttons[1].OnClick += () =>
        {
            GameStarter.Game.StartLevel--;
            LevelText.text = GameStarter.Game.StartLevel.ToString();
        };
        Buttons[2].OnClick += () =>
        {
            GameStarter.Game.StartGame();
            SetMenu(null as Menu);
        };
    }
}