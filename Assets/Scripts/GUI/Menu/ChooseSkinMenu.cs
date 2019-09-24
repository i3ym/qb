using System;
using System.Linq;

public class ChooseSkinMenu : ChooseMenu
{
    protected override Type PreviousMenuType => typeof(MainMenu);

    protected override(string name, Action onSelect, Action onClick) [] GetButtons()
    {
        return new []
        {
            "Classic",
            "Sadness",
            "Forest",
            "Lines",
            "Faces",
            "Neon",
            "Leaf Fall",
            "Hot Dog",
            "Vending Machine"
        }.Select((string x, int i) => (x,
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