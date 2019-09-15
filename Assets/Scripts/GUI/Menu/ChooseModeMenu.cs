using System;

public class ChooseModeMenu : ChooseMenu
{
    protected override(string name, Action onSelect, Action onClick) [] GetButtons() =>
        new(string, Action, Action) []
        {
            ("Normal",
            () => Game.BlindRendering = false,
            () => SetMenu<ChooseLvlMenu>()),
            ("Blind",
            () => Game.BlindRendering = true,
            () => SetMenu<ChooseLvlMenu>())
        };
}