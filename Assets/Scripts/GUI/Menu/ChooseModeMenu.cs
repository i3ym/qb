using System;
using qb.Shared;

public class ChooseModeMenu : ChooseMenu
{
    protected override((string rus, string eng) name, Action onSelect, Action onClick) [] GetButtons() =>
        new((string, string), Action, Action) []
        {
            (("Обычный", "Normal"),
            () => FieldController.BlindRendering = false,
            () => SetMenu<ChooseLvlMenu>()),

            (("Слепой", "Blind"),
            () => FieldController.BlindRendering = true,
            () => SetMenu<ChooseLvlMenu>())
        };
}