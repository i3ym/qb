using System.Collections.Generic;
using System.Linq;
using qb.Shared;
using qb.Shared.Mods;
using UnityEngine;

namespace qb.Unity
{
    public class UnitySelectableMenuMod : UnitySelectableMenu<Mod> { }

    public class UnitySelectableMenu<T> : UnityMenu, ISelectableMenu<T>
    {
        public IReadOnlyList<T> SelectedButtons => _SelectedButtons.AsReadOnly();
        List<T> _SelectedButtons = new List<T>();

        public override IReadOnlyList<IMenuButton> Buttons
        {
            get => base.Buttons;
            set
            {
                base.Buttons = value;

                foreach (var button in value.OfType<ISelectableMenuButton<T>>().Where(x => x.IsClickable))
                {
                    var btn = button;

                    (btn as UnityMenuButton).TranslatedText.RawImage.color = new Color(.5f, .5f, .5f, 1);

                    btn.OnClick += () =>
                    {
                        if (_SelectedButtons.Contains(btn.SelectableValue))
                        {
                            _SelectedButtons.Remove(btn.SelectableValue);
                            (btn as UnityMenuButton).TranslatedText.RawImage.color = new Color(.5f, .5f, .5f, 1);
                        }
                        else
                        {
                            _SelectedButtons.Add(btn.SelectableValue);
                            (btn as UnityMenuButton).TranslatedText.RawImage.color = new Color(1, 1, 1, 1);
                        }
                    };
                }
            }
        }
    }
}