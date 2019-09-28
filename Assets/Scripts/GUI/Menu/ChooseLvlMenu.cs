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
            Game.Instance.StartLevel++;
            LevelText.text = Game.Instance.StartLevel.ToString();
        };
        Buttons[1].OnClick += () =>
        {
            Game.Instance.StartLevel--;
            LevelText.text = Game.Instance.StartLevel.ToString();
        };
        Buttons[2].OnClick += () =>
        {
            Game.Instance.StartGame();
            SetMenu(null as Menu);
        };
    }
}