using osu.Framework.Graphics;

namespace qb.Gui
{
    public class ControllerButton : PixelButton
    {
        public bool IsPressed => IsMouseOverPressed;

        public ControllerButton() => Size = new osuTK.Vector2(1);
        public ControllerButton(Anchor anchor) : this() => Anchor = Origin = anchor;
    }
}