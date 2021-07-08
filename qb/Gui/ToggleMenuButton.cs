using System;

namespace qb.Gui
{
    public class ToggleMenuButton<T> : MenuButton
    {
        public readonly T Value;

        public ToggleMenuButton(T value) : base() => Value = value;
        public ToggleMenuButton(T value, TranslatedString text) : this(value) => TranslatedText = text;
        public ToggleMenuButton(T value, TranslatedString text, Action action) : base(text, action) => Value = value;
    }
}