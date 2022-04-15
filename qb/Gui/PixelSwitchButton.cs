namespace qb.Gui
{
    public class PixelSwitchButton : PixelTextButton
    {
        readonly Bindable<bool> Bindable;

        public PixelSwitchButton(TranslatedString text, Bindable<bool> bindable) : base(text)
        {
            Bindable = bindable = bindable.GetBoundCopy();
            Bindable.BindValueChanged(e => PixelText.FadeColour(e.NewValue ? Colour4.White : Colour4.Gray, 250, Easing.OutQuad));

            Action = () => Bindable.Value = !bindable.Value;
        }
    }
}