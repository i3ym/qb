using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    static List<Menu> Menus = new List<Menu>();

    public static Menu ActiveController { get; protected set; }
    public static Button ActiveButton => ActiveController.Buttons[ActiveController.ActiveButtonIndex];

    [SerializeField]
    protected Button[] Buttons;

    protected bool IsLocked
    {
        get => _IsLocked;
        set
        {
            _IsLocked = value;

            foreach (var btn in Buttons)
                btn.GetComponent<TextMeshProUGUI>().color = value ? Color.gray : Color.white;
        }
    }

    bool _IsLocked = false;

    protected int ActiveButtonIndex = 0;

    protected abstract Type PreviousMenuType { get; }

    static Menu()
    {
        PlayerControls.OnUp += () =>
        {
            if (ActiveController == null || ActiveController.IsLocked || !ActiveController.enabled || !ActiveController.gameObject.activeSelf) return;

            ActiveController.ActiveButtonIndex--;

            if (ActiveController.ActiveButtonIndex < 0)
                ActiveController.ActiveButtonIndex = ActiveController.Buttons.Length - 1;

            while (!ActiveController.Buttons[ActiveController.ActiveButtonIndex].gameObject.activeSelf)
            {
                ActiveController.ActiveButtonIndex--;

                if (ActiveController.ActiveButtonIndex < 0)
                    ActiveController.ActiveButtonIndex = ActiveController.Buttons.Length - 1;
            }

            UpdateButtonDesign();
        };
        PlayerControls.OnDown += () =>
        {
            if (ActiveController == null || ActiveController.IsLocked || !ActiveController.enabled || !ActiveController.gameObject.activeSelf) return;

            ActiveController.ActiveButtonIndex = (ActiveController.ActiveButtonIndex + 1) % ActiveController.Buttons.Length;

            while (!ActiveController.Buttons[ActiveController.ActiveButtonIndex].gameObject.activeSelf)
                ActiveController.ActiveButtonIndex = (ActiveController.ActiveButtonIndex + 1) % ActiveController.Buttons.Length;

            UpdateButtonDesign();
        };
        PlayerControls.OnSelect += () =>
        {
            if (ActiveController == null || ActiveController.IsLocked || !ActiveController.enabled || !ActiveController.gameObject.activeSelf) return;
            ActiveButton.OnClick();
        };
        PlayerControls.OnBack += () =>
        {
            if (ActiveController == null || ActiveController.IsLocked || !ActiveController.enabled || !ActiveController.gameObject.activeSelf) return;
            ActiveController.OnGoBack();
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
    protected virtual void OnShow() { }
    protected virtual void OnClose() { }
    protected virtual void OnGoBack() { }

    public static T GetMenu<T>() where T : Menu => Menus.OfType<T>().First();
    public static void SetMenu(Menu menu)
    {
        foreach (var m in Menus) m.gameObject.SetActive(false);

        BottomLine.Instance.enabled = !(TopImage.Instance.enabled =
            menu == null || menu is PauseMenu || menu is GameOverMenu || menu is ChooseMenu);

        if (menu != null) menu.Enable();

        if (ActiveController != null) ActiveController.OnClose();
        ActiveController = menu;
        UpdateButtonDesign();

        Controller.controller.Select.gameObject.SetActive(menu != null);
        Controller.controller.Back.gameObject.SetActive(menu != null);
        Controller.controller.Rotate.gameObject.SetActive(menu == null);
        Controller.controller.RotateC.gameObject.SetActive(menu == null);
    }
    public static void SetMenu(Type type) => SetMenu(type == null ? null as Menu : Menus.First(x => x.GetType() == type));
    public static void SetMenu<T>() where T : Menu => SetMenu(typeof(T));

    protected void Enable()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    protected static void UpdateButtonDesign()
    {
        if (ActiveController == null) return;

        ActiveButton.OnDeselect();
        ButtonDesign.Design.transform.SetParent(ActiveButton.transform, false);
        ActiveButton.OnSelect();
    }
}