using System;
using System.Linq;

public class ChooseSkinMenu : ChooseMenu
{
    protected override(string name, Action onSelect, Action onClick) [] GetButtons()
    {
        return new []
        {
            "Classic",
            "Sadness",
            "Forest",
            "TODO",
            "Faces",
            "Neon"
        }.Select((string x, int i) => (x, (Action) (() => FieldObjectRenderer.Skin = i + 1), (Action) (() => FieldObjectRenderer.Skin = i + 1))).ToArray();
    }
}