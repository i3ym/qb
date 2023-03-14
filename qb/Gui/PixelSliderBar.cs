namespace qb.Gui
{
    public partial class PixelSliderBar : BasicSliderBar<float>, INavigatableButton
    {
        bool INavigatableButton.Focusable => true;
        public new BindableNumber<float> CurrentNumber => base.CurrentNumber;

        public PixelSliderBar(TranslatedString text, float min, float max, Action<ValueChangedEvent<float>> func)
        {
            CurrentNumber.MinValue = min;
            CurrentNumber.MaxValue = max;
            Current.ValueChanged += func;

            SelectionColour = Colour4.White;
            BackgroundColour = Colour4.White.MultiplyAlpha(.5f);

            AddInternal(new PixelText(text) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Colour = Colour4.Black });
        }

        void INavigatableButton.OnEvent(QbKey action)
        {
            var diff = (CurrentNumber.MaxValue - CurrentNumber.MinValue) / 10f;

            if (action == QbKey.Left) CurrentNumber.Value -= diff;
            else if (action == QbKey.Right) CurrentNumber.Value += diff;
        }
    }
}