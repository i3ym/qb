using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseModeMenu : Menu
{
    public static int MaxX, MaxY;

    [SerializeField]
    GameObject ButtonPrefab = null;

    new RectTransform transform;
    List<RenderingType> Types = new List<RenderingType>();

    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override void Init()
    {
        transform = gameObject.transform as RectTransform;

        Types.Add(new RenderingType("Normal", () => Game.BlindRendering = false));
        Types.Add(new RenderingType("Blind", () => Game.BlindRendering = true));
        Types.Add(new RenderingType("TESTNormal", () => Game.BlindRendering = false));
        Types.Add(new RenderingType("TESTBlind", () => Game.BlindRendering = true));

        Buttons = new Button[Types.Count];

        MaxX = (int) transform.rect.width;
        MaxY = (int) transform.rect.height - (Buttons.Length - 1) * 2;

        TextMeshProUGUI text;
        RectTransform tr;
        GameObject go;
        Button btn;
        for (int i = 0; i < Types.Count; i++)
        {
            go = Instantiate(ButtonPrefab, Vector3.zero, Quaternion.identity, transform);
            btn = go.GetComponentInChildren<Button>();
            tr = go.transform as RectTransform;

            text = btn.GetComponent<TextMeshProUGUI>();
            text.text = Types[i].Name;
            (text.transform as RectTransform).sizeDelta = new Vector2(text.text.Length, (text.transform as RectTransform).sizeDelta.y);

            tr.anchorMin = new Vector2(0f, 1f);
            tr.anchorMax = new Vector2(1f, 1f);
            tr.sizeDelta = new Vector2(tr.sizeDelta.x, transform.rect.height / Types.Count);
            tr.anchoredPosition = new Vector2(0f, -transform.rect.height / Types.Count * i);

            int index = i;
            btn.OnSelect = () =>
            {
                Types[index].OnSelect();
                ShowField(index);
            };
            btn.OnClick += () =>
            {
                Types[index].OnClick();
                Game.Instance.RestartGame();
                SetMenu(null as Menu);
            };

            Buttons[i] = btn;
        }
    }

    void ShowField(int selectedIndex)
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