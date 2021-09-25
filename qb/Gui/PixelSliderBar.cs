using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace qb.Gui
{
    public class PixelSliderBar : BasicSliderBar<float>
    {
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
    }
}