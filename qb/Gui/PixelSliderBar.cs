using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace qb.Gui
{
    public class PixelSliderBar : BasicSliderBar<float>
    {
        public new BindableNumber<float> CurrentNumber => base.CurrentNumber;

        public PixelSliderBar(float min, float max)
        {
            CurrentNumber.MinValue = min;
            CurrentNumber.MaxValue = max;

            SelectionColour = Colour4.White;
            BackgroundColour = Colour4.White.MultiplyAlpha(.5f);
        }
    }
}