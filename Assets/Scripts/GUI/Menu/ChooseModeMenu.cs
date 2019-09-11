using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseModeMenu : ChooseMenu
{
    protected override(string name, Action onSelect, Action onClick) [] GetButtons()
    {
        return new(string, Action, Action) []
        {
            ("Normal", () => Game.BlindRendering = false, delegate { }),
            ("Blind", () => Game.BlindRendering = true, delegate { })
        };
    }
}