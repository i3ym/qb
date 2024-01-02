namespace qb.Gui;

public partial class PixelSwitchButton : PixelTextButton
{
    public PixelSwitchButton(TranslatedString text, Bindable<bool> bindable) : base(text)
    {
        bindable = bindable.GetBoundCopy();
        bindable.BindValueChanged(e => PixelText.FadeColour(e.NewValue ? Colour4.White : Colour4.Gray, 250, Easing.OutQuad));

        Action = () => bindable.Value = !bindable.Value;
    }
    public PixelSwitchButton(TranslatedString text, Func<bool> getfunc, Action<bool> setfunc) : base(text)
    {
        bool value = getfunc();
        Action = () => update(value = !value);
        update(value);

        void update(bool value)
        {
            setfunc(value);
            PixelText.FadeColour(value ? Colour4.White : Colour4.Gray, 250, Easing.OutQuad);
        }
    }
}
