using System;
using System.Linq;

public class ChooseSkinMenu : ChooseMenu
{
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override((string rus, string eng) name, Action onSelect, Action onClick) [] GetButtons()
    {
        return new []
        {
            ("Классический", "Classic"),
            ("Печаль", "Sadness"),
            ("Лес", "Forest"),
            ("Линии", "Lines"),
            ("Лица", "Faces"),
            ("Неон", "Neon"),
            ("Листопад", "Leaf Fall"),
            ("Хот Дог", "Hot Dog"),
            ("Торговый\nАвтомат", "Vending\nMachine"),
        }.Select(((string rus, string eng) x, int i) => (x,
            (Action) (() => Config.SelectedSkin = i),
            (Action) (() => SetMenu<MainMenu>()))).ToArray();
    }

    protected override void OnShow()
    {
        base.OnShow();

        ActiveButtonIndex = Config.SelectedSkin;
        UpdateButtonDesign();
    }
}