using System;
using osu.Framework.Input.Events;

namespace qb.Gui
{
    public class MenuButton : PixelText
    {
        public Action Action = delegate { };

        public MenuButton() { }
        public MenuButton(TranslatedString text, Action action)
        {
            TranslatedText = text;
            Action = action;
        }

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);
            Action();

            return true;
        }
    }
}