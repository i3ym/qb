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
            Game.Instance.StartLvl++;

            if (Game.Instance.StartLvl > 30) Game.Instance.StartLvl = 30;

            LevelText.text = Game.Instance.StartLvl.ToString();
        };
        Buttons[1].OnClick += () =>
        {
            Game.Instance.StartLvl--;

            if (Game.Instance.StartLvl < 0) Game.Instance.StartLvl = 0;

            LevelText.text = Game.Instance.StartLvl.ToString();
        };
        Buttons[2].OnClick += () =>
        {
            Game.Instance.RestartGame();
            SetMenu(null as Menu);
        };
    }
}