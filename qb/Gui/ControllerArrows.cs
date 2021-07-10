using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace qb.Gui
{
    public class ControllerArrows : CompositeDrawable
    {
        [Resolved(name: nameof(PixelTextureStore))] TextureStore PixelTextureStore { get; init; } = null!;

        [BackgroundDependencyLoader]
        void Load()
        {
            AddInternal(new Sprite()
            {
                RelativeSizeAxes = Axes.Both,
                Texture = PixelTextureStore.Get("Resources/ControllerArrows"),
            });


            add(QbKey.Up, Anchor.TopCentre);
            add(QbKey.Down, Anchor.BottomCentre);
            add(QbKey.Left, Anchor.CentreLeft);
            add(QbKey.Right, Anchor.CentreRight);

            void add(QbKey key, Anchor anchor) =>
                AddInternal(new ControllerButton(key, anchor) { RelativeSizeAxes = Axes.Both, Size = new(1 / 3f) });
        }
    }
}