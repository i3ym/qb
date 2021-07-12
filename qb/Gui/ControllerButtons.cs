using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using qb.Input;

namespace qb.Gui
{
    public class ControllerButtons : CompositeDrawable, IController
    {
        readonly ControllerButton A, B, Back;

        public ControllerButtons()
        {
            A = new ControllerButton(Anchor.BottomLeft) { Text = "A", RelativeSizeAxes = Axes.Both, Size = new(1 / 2.1f) };
            B = new ControllerButton(Anchor.BottomRight) { Text = "B", RelativeSizeAxes = Axes.Both, Size = new(1 / 2.1f) };
            Back = new ControllerButton() { Text = ("Back", "Назад"), RelativeSizeAxes = Axes.Both, Width = 1, Height = (1 / 2.1f) };
        }

        [BackgroundDependencyLoader]
        void Load()
        {
            AddInternal(A);
            AddInternal(B);
            AddInternal(Back);
        }

        public ControllerState GetState() =>
            new ControllerState()
            {
                A = A.IsPressed,
                B = B.IsPressed,
                Back = Back.IsPressed,
            };
    }
}