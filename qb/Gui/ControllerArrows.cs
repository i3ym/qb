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

            AddInternal(new ControllerButton(Anchor.CentreLeft));
            AddInternal(new ControllerButton(Anchor.TopCentre));
            AddInternal(new ControllerButton(Anchor.CentreRight));
            AddInternal(new ControllerButton(Anchor.BottomCentre));
        }
    }
}