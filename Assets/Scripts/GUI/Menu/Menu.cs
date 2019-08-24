using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    static List<Menu> Menus = new List<Menu>();

    public static Menu ActiveController { get; protected set; }
    public static Button ActiveButton => ActiveController.Buttons[ActiveController.ActiveButtonIndex];

    [SerializeField]
    protected Button[] Buttons;

    int ActiveButtonIndex = 0;

    protected abstract Type PreviousMenuType { get; }

    static Menu()
    {
        PlayerControls.OnUp += () =>
        {
            if (ActiveController == null || !ActiveController.enabled || !ActiveController.gameObject.activeSelf) return;

            ActiveController.ActiveButtonIndex--;
            if (ActiveController.ActiveButtonIndex < 0) ActiveController.ActiveButtonIndex = ActiveController.Buttons.Length - 1;
            UpdateButtonDesign();
        };
        PlayerControls.OnDown += () =>
        {
            if (ActiveController == null || !ActiveController.enabled || !ActiveController.gameObject.activeSelf) return;

            ActiveController.ActiveButtonIndex = (ActiveController.ActiveButtonIndex + 1) % ActiveController.Buttons.Length;
            UpdateButtonDesign();
        };
        PlayerControls.OnRotate += () =>
        {
            if (ActiveController == null || !ActiveController.enabled || !ActiveController.gameObject.activeSelf) return;
            ActiveButton.OnClick();
        };
        PlayerControls.OnRotateC += () =>
        {
            if (ActiveController == null || !ActiveController.enabled || !ActiveController.gameObject.activeSelf) return;
            SetMenu(ActiveController.PreviousMenuType);
        };
    }

    void Awake()
    {
        Menus.Add(this);
        gameObject.SetActive(false);

        Init();
    }

    protected abstract void Init();

    public static void SetMenu(Menu menu)
    {
        foreach (var m in Menus) m.gameObject.SetActive(false);

        if (menu != null) menu.Enable();
        ActiveController = menu;
        UpdateButtonDesign();
    }
    public static void SetMenu(Type type) => SetMenu(Menus.First(x => x.GetType() == type));
    public static void SetMenu<T>() where T : Menu => SetMenu(typeof(T));

    protected void Enable() => gameObject.SetActive(true);

    protected static void UpdateButtonDesign()
    {
        if (ActiveController == null) return;

        ActiveButton.OnDeselect();
        ButtonDesign.Design.transform.SetParent(ActiveButton.transform, false);
        ActiveButton.OnSelect();
    }
}