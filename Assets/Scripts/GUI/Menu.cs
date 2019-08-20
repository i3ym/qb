using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Menu : ButtonController
{
    [SerializeField]
    public static Menu[] eEEE;
    static List<Menu> Menus = new List<Menu>();

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
    public static void SetMenu<T>() where T : Menu => SetMenu(Menus.First(x => x.GetType() == typeof(T)));
}