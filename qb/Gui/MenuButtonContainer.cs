using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class MenuButtonContainer : MenuButtonContainer<MenuButton> { }
    public class MenuButtonContainer<T> : Container<T> where T : MenuButton
    {
        public Anchor ChildAxes { get => PixelContent.ChildAxes; set => PixelContent.ChildAxes = value; }

        readonly PixelFlowContainer<T> PixelContent = new PixelFlowContainer<T>() { AutoSizeAxes = Axes.Both, Direction = FillDirection.Vertical, Spacing = new(1) };
        protected override Container<T> Content => PixelContent;

        [BackgroundDependencyLoader]
        void Load()
        {
            InternalChild = Content;
        }

        public override void Add(T drawable)
        {
            drawable.Anchor = drawable.Origin = Anchor.TopCentre;
            base.Add(drawable);
        }
    }
}