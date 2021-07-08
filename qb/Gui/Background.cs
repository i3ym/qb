using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;

namespace qb.Gui
{
    public class Background : Box
    {
        public Background() => RelativeSizeAxes = Axes.Both;
        public Background(Colour4 colour) : this() => Colour = colour;
    }
}