using System;

public class ChooseModeMenu : ChooseMenu
{
    protected override((string rus, string eng) name, Action onSelect, Action onClick) [] GetButtons() =>
        new((string, string), Action, Action) []
        {
            (("Обычный", "Normal"),
            () => Game.BlindRendering = false,
            () => SetMenu<ChooseLvlMenu>()),

            (("Слепой", "Blind"),
            () => Game.BlindRendering = true,
            () => SetMenu<ChooseLvlMenu>())
        };
}