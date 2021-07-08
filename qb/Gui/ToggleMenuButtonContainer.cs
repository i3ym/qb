using System;
using System.Collections.Generic;
using osu.Framework.Graphics;

namespace qb.Gui
{
    public class ToggleMenuButtonContainer<T> : MenuButtonContainer<ToggleMenuButton<T>>
    {
        public Action<T> OnEnable = delegate { };
        public Action<T> OnDisable = delegate { };

        public IReadOnlyList<T> Selected => _Selected;
        readonly List<T> _Selected = new();

        public override void Add(ToggleMenuButton<T> drawable)
        {
            void fade(bool enabled, float duration = 250) => drawable.FadeTo(enabled ? 1f : .5f, duration, Easing.OutQuad);

            drawable.Action += () =>
            {
                var enabled = _Selected.Contains(drawable.Value);

                if (enabled)
                {
                    _Selected.Remove(drawable.Value);
                    OnDisable(drawable.Value);
                }
                else
                {
                    _Selected.Add(drawable.Value);
                    OnEnable(drawable.Value);
                }


                fade(!enabled);
            };
            fade(false, 0);


            base.Add(drawable);
        }
    }
}