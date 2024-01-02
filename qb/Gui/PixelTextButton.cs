namespace qb.Gui;

public partial class PixelTextButton : PixelButton<PixelText>
{
    public PixelText PixelText => Child!;
    public TranslatedString Text { get => PixelText.TranslatedText; set => PixelText.TranslatedText = value; }

    public PixelTextButton() : base(new PixelText() { Anchor = Anchor.Centre, Origin = Anchor.Centre }) { }
    public PixelTextButton(TranslatedString text) : this() => PixelText.TranslatedText = text;
    public PixelTextButton(TranslatedString text, Action action) : this(text) => Action = action;
}
