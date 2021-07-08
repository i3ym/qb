using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class MenuButtonContainer : MenuButtonContainer<MenuButton> { }
    public class MenuButtonContainer<T> : Container<T> where T : MenuButton
    {
        protected override Container<T> Content { get; } = new PixelFlowContainer<T>() { RelativeSizeAxes = Axes.X, AutoSizeAxes = Axes.Y, Direction = FillDirection.Vertical, Spacing = new(1) };

        [BackgroundDependencyLoader]
        void Load()
        {
            InternalChild = new PixelScrollContainer() { RelativeSizeAxes = Axes.Both, Child = Content, ScrollDistance = 2f };
        }

        public override void Add(T drawable)
        {
            drawable.Anchor = drawable.Origin = Anchor.TopCentre;
            base.Add(drawable);
        }
    }
}