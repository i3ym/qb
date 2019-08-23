using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Menu : ButtonController
{
    static List<Menu> Menus = new List<Menu>();

    protected abstract Type PreviousMenuType { get; }

    void Awake()
    {
        Menus.Add(this);
        gameObject.SetActive(false);

        Controller.controller.RotateC.OnMouseDownAction += () =>
        {
            if (enabled && gameObject.activeSelf && ActiveController == this)
                SetMenu(PreviousMenuType);
        };

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
}