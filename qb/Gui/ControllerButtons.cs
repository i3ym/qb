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
            A = new ControllerButton(Anchor.BottomLeft) { Text = "A", RelativeSizeAxes = Axes.Both, Size = new osuTK.Vector2(1 / 2.1f) };
            B = new ControllerButton(Anchor.BottomRight) { Text = "B", RelativeSizeAxes = Axes.Both, Size = new osuTK.Vector2(1 / 2.1f) };
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
            new ControllerState
            (
                a: A.IsPressed,
                b: B.IsPressed,
                back: Back.IsPressed
            );
    }
}