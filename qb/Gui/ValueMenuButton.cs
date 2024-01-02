namespace qb.Gui;

public partial class ValueMenuButton<T> : PixelTextButton
{
    public readonly T Value;

    public ValueMenuButton(T value) : base() => Value = value;
    public ValueMenuButton(T value, TranslatedString text) : this(value) => PixelText.TranslatedText = text;
    public ValueMenuButton(T value, TranslatedString text, Action action) : base(text, action) => Value = value;
}
