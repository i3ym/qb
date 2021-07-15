using osu.Framework.Allocation;
using osu.Framework.Graphics;
using qb.Input;

namespace qb.Gui
{
    public class ControllerButtons : ControllerButtonsBase, IController
    {
        readonly ControllerButton A, B, Back;

        public ControllerButtons()
        {
            A = create("A", Anchor.BottomRight);
            B = create("B", Anchor.BottomLeft);
            Back = create(("Back", "Назад"), fontSize: .2f).With(x => x.Width = 1);

            ControllerButton create(TranslatedString text, Anchor anchor = Anchor.TopLeft, float fontSize = .4f) =>
                new ControllerButton(anchor) { Text = text, Background = Colour4.White, TextColour = Colour4.Black, RelativeSizeAxes = Axes.Both, Size = new osuTK.Vector2(1 / 2.1f) }
                .With(x => x.PixelText.Font = x.PixelText.Font.With(size: fontSize));
        }

        [BackgroundDependencyLoader]
        void Load()
        {
            AddInternal(A);
            AddInternal(B);
            AddInternal(Back);
        }

        public ControllerState GetState() =>
            new ControllerState
            (
                a: A.IsPressed,
                b: B.IsPressed,
                back: Back.IsPressed
            );
    }
}