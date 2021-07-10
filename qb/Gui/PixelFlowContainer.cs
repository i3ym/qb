using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class PixelFlowContainer : PixelFlowContainer<Drawable> { }
    public class PixelFlowContainer<T> : FillFlowContainer<T> where T : Drawable
    {
        public Anchor ChildAxes { get => ChildAxesBindable.Value; set => ChildAxesBindable.Value = value; }
        public readonly Bindable<Anchor> ChildAxesBindable = new(Anchor.TopCentre);

        [BackgroundDependencyLoader]
        void Load()
        {
            Spacing = new(1, 0);
            ChildAxesBindable.BindValueChanged(e =>
            {
                foreach (var child in Children)
                    child.Anchor = child.Origin = e.NewValue;
            }, true);
        }
    }
}