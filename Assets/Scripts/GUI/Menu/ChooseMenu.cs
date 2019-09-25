using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ChooseMenu : Menu
{
    [SerializeField]
    protected GameObject ButtonPrefab = null;

    protected new RectTransform transform;
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {
        transform = gameObject.transform as RectTransform;

        var buttons = GetButtons();
        Buttons = new Button[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
            CreateButton(i, buttons[i].name, buttons[i].onSelect, buttons[i].onClick);
    }

    protected abstract(string name, Action onSelect, Action onClick) [] GetButtons();

    protected void CreateButton(int index, string name, Action onSelect, Action onClick)
    {
        var go = Instantiate(ButtonPrefab, Vector3.zero, Quaternion.identity, transform);
        var btn = go.GetComponentInChildren<Button>();
        var tr = go.transform as RectTransform;

        var text = btn.GetComponent<TextMeshProUGUI>();
        text.text = name;
        (btn.transform as RectTransform).sizeDelta = new Vector2(text.text.Length, (btn.transform as RectTransform).sizeDelta.y);

        tr.localPosition -= new Vector3(0f, 0f, tr.localPosition.z);

        btn.OnSelect += () =>
        {
            onSelect();
            ShowField(index);
        };
        btn.OnClick += () => onClick();

        Buttons[index] = btn;
    }

    protected override void OnShow() => AutoGame.Instance.StartGame();
    protected override void OnClose() => AutoGame.Instance.StopGame();

    protected virtual void ShowField(int selectedIndex)
    {
        AutoGame.UpdateField();

        RectTransform tr;
        for (int i = 0; i < Buttons.Length; i++)
        {
            tr = (Buttons[i].transform as RectTransform);
            tr.anchoredPosition = new Vector2(0f, -(i - selectedIndex) * 3f);
        }
    }
}