using osu.Framework.Graphics;

namespace qb.Gui
{
    public class ControllerButton : PixelButton
    {
        public bool IsPressed { get; private set; }

        public ControllerButton()
        {
            Size = new osuTK.Vector2(1);

            MouseDown += () => IsPressed = true;
            MouseUp += () => IsPressed = false;
        }
        public ControllerButton(Anchor anchor) : this() => Anchor = Origin = anchor;
    }
}