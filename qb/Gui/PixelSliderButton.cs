using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace qb.Gui
{
    public class PixelSliderButton : PixelButton<PixelSliderBar>
    {
        public BindableNumber<float> Current => Child!.CurrentNumber;

        public PixelSliderButton(TranslatedString text, float min, float max, Action<ValueChangedEvent<float>> func) : base(new PixelSliderBar(min, max) { RelativeSizeAxes = Axes.Both })
        {
            RelativeSizeAxes = Axes.X;

            Child.Current.ValueChanged += func;
            AddInternal(new PixelText(text) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Colour = Colour4.Black });
        }
    }
}