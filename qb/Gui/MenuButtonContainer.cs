using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class MenuButtonContainer : Container
    {
        protected override Container<Drawable> Content { get; } = new FillFlowContainer<Drawable>() { RelativeSizeAxes = Axes.X, AutoSizeAxes = Axes.Y, Direction = FillDirection.Vertical, Spacing = new osuTK.Vector2(0, 1) };

        [BackgroundDependencyLoader]
        void Load()
        {
            InternalChild = new BasicScrollContainer<Drawable>() { RelativeSizeAxes = Axes.Both, Child = Content, ScrollDistance = 2f };
        }

        public override void Add(Drawable drawable)
        {
            drawable.Anchor = drawable.Origin = Anchor.TopCentre;
            base.Add(drawable);
        }
    }
}