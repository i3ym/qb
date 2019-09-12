using System;
using System.Linq;

public class ChooseSkinMenu : ChooseMenu
{
    protected override Type PreviousMenuType => typeof(SettingsMenu);

    protected override(string name, Action onSelect, Action onClick) [] GetButtons()
    {
        return new []
        {
            "Classic",
            "Sadness",
            "Forest",
            "Lines",
            "Faces",
            "Neon"
        }.Select((string x, int i) => (x,
            (Action) (() => FieldObjectRenderer.Skin = i + 1),
            (Action) (() => SetMenu<SettingsMenu>()))).ToArray();
    }

    protected override void OnShow()
    {
        ActiveButtonIndex = Config.SelectedSkin - 1;
        UpdateButtonDesign();
    }
}