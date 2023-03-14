namespace qb.Gui
{
    public partial class Background : Box
    {
        public Background() => RelativeSizeAxes = Axes.Both;
        public Background(Colour4 colour) : this() => Colour = colour;
    }
}