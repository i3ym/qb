using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class PixelFlowContainer : PixelFlowContainer<Drawable> { }
    public class PixelFlowContainer<T> : FillFlowContainer<T> where T : Drawable
    {
        [BackgroundDependencyLoader]
        void Load() => Spacing = new(1);
    }
}