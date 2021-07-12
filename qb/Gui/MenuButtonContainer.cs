using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class MenuButtonContainer : MenuButtonContainer<PixelButton> { }
    public class MenuButtonContainer<T> : Container<T> where T : PixelButton
    {
        public Anchor ChildrenAnchor { get => PixelContent.ChildAxes; set => PixelContent.ChildAxes = value; }

        readonly PixelFlowContainer<T> PixelContent = new PixelFlowContainer<T>() { AutoSizeAxes = Axes.Both, Direction = FillDirection.Vertical, Spacing = new osuTK.Vector2(1) };
        protected override Container<T> Content => PixelContent;

        [BackgroundDependencyLoader]
        void Load()
        {
            InternalChild = Content;
        }
    }
}