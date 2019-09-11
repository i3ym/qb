using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ChooseMenu : Menu
{
    public static int MaxX, MaxY;

    [SerializeField]
    protected GameObject ButtonPrefab = null;

    protected new RectTransform transform;
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {
        transform = gameObject.transform as RectTransform;

        MaxX = (int) transform.rect.width;
        MaxY = (int) transform.rect.height - (Buttons.Length - 1) * 2;

        var buttons = GetButtons();
        Buttons = new Button[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
            CreateButton(i, buttons.Length, buttons[i].name, buttons[i].onSelect, buttons[i].onClick);
    }

    protected abstract(string name, Action onSelect, Action onClick) [] GetButtons();

    protected void CreateButton(int index, int max, string name, Action onSelect, Action onClick)
    {
        var go = Instantiate(ButtonPrefab, Vector3.zero, Quaternion.identity, transform);
        var btn = go.GetComponentInChildren<Button>();
        var tr = go.transform as RectTransform;

        var text = btn.GetComponent<TextMeshProUGUI>();
        text.text = name;
        (btn.transform as RectTransform).sizeDelta = new Vector2(text.text.Length, (btn.transform as RectTransform).sizeDelta.y);

        tr.anchorMin = new Vector2(0f, 1f);
        tr.anchorMax = new Vector2(1f, 1f);
        tr.sizeDelta = new Vector2(tr.sizeDelta.x, transform.rect.height / max);
        tr.anchoredPosition = new Vector2(0f, -transform.rect.height / max * index);
        tr.localPosition -= new Vector3(0f, 0f, tr.localPosition.z);

        btn.OnSelect += () =>
        {
            onSelect();
            ShowField(index);
        };
        btn.OnClick += () =>
        {
            onClick();
            Game.Instance.RestartGame();
            SetMenu(null as Menu);
        };

        var autoGame = go.GetComponent<AutoGame>();
        autoGame.SetMax(MaxX, MaxY);

        Buttons[index] = btn;
    }

    protected void ShowField(int selectedIndex)
    {
        RectTransform tr;
        for (int i = 0; i < Buttons.Length; i++)
        {
            tr = (Buttons[i].transform.parent as RectTransform);

            tr.sizeDelta =
                i == selectedIndex ?
                new Vector2(tr.sizeDelta.x, transform.rect.height - (Buttons.Length - 1) * 2) :
                new Vector2(tr.sizeDelta.x, 2f);

            if (i <= selectedIndex) tr.anchoredPosition = new Vector2(tr.anchoredPosition.x, -i * 2);
            else tr.anchoredPosition = new Vector2(tr.anchoredPosition.x, -(i * 2 - 2 + (transform.rect.height - (Buttons.Length - 1) * 2)));

            if (i == selectedIndex)
            {
                tr.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                tr.GetComponent<AutoGame>().StartGame();
            }
            else
            {
                tr.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
                tr.GetComponent<AutoGame>().StopGame();
            }
        }
    }
}