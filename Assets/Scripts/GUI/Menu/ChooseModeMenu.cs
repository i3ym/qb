using System;

public class ChooseModeMenu : ChooseMenu
{
    protected override(string name, Action onSelect, Action onClick) [] GetButtons() =>
        new(string, Action, Action) []
        {
            ("Normal",
                () => Game.BlindRendering = false, () =>
                {
                    Game.Instance.RestartGame();
                    SetMenu(null as Menu);
                }),
            ("Blind",
                () => Game.BlindRendering = true,
                () =>
                {
                    Game.Instance.RestartGame();
                    SetMenu(null as Menu);
                })
        };
}