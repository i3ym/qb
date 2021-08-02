using osu.Framework.Graphics;

namespace qb.Gui
{
    public class ControllerButtons : ControllerButtonsBase
    {
        readonly ControllerButton A, B, Back;

        public ControllerButtons()
        {
            A = create(QbKey.A, "A", Anchor.BottomRight).With(AddInternal);
            B = create(QbKey.B, "B", Anchor.BottomLeft).With(AddInternal);
            Back = create(QbKey.Back, ("Back", "Назад"), fontSize: .2f).With(x => x.Width = 1).With(AddInternal);

            ControllerButton create(QbKey key, TranslatedString text, Anchor anchor = Anchor.TopLeft, float fontSize = .4f) =>
                new ControllerButton(key, anchor) { Text = text, Background = Colour4.White, TextColour = Colour4.Black, RelativeSizeAxes = Axes.Both, Size = new osuTK.Vector2(1 / 2.1f) }
                .With(x => x.PixelText.Font = x.PixelText.Font.With(size: fontSize));
        }
    }
}