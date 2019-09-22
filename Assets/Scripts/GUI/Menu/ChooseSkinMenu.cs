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
            "Autumn",
            "Hot Dog"
        }.Select((string x, int i) => (x,
            (Action) (() => Config.SelectedSkin = i),
            (Action) (() => SetMenu<MainMenu>()))).ToArray();
    }

    protected override void OnShow()
    {
        ActiveButtonIndex = Config.SelectedSkin;
        UpdateButtonDesign();
    }
}