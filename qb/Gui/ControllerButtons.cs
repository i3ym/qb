using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class ControllerButtons : CompositeDrawable, IController
    {
        readonly ControllerButton A, B, Back;

        public ControllerButtons()
        {
            A = new ControllerButton() { RelativeSizeAxes = Axes.Both, Size = new(1 / 3f) };
            B = new ControllerButton() { RelativeSizeAxes = Axes.Both, Size = new(1 / 3f) };
            Back = new ControllerButton() { RelativeSizeAxes = Axes.Both, Size = new(1 / 3f) };
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