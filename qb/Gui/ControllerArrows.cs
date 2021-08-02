using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace qb.Gui
{
    public class ControllerArrows : ControllerButtonsBase
    {
        [Resolved(name: nameof(PixelTextureStore))] TextureStore PixelTextureStore { get; set; } = null!;

        readonly ControllerButton Up, Down, Left, Right;

        public ControllerArrows()
        {
            Up = create(QbKey.Up, Anchor.TopCentre).With(AddInternal);
            Down = create(QbKey.Down, Anchor.BottomCentre).With(AddInternal);
            Left = create(QbKey.Left, Anchor.CentreLeft).With(AddInternal);
            Right = create(QbKey.Right, Anchor.CentreRight).With(AddInternal);

            ControllerButton create(QbKey key, Anchor anchor) => new ControllerButton(key, anchor) { RelativeSizeAxes = Axes.Both, Size = new osuTK.Vector2(1 / 2.5f) };
        }

        [BackgroundDependencyLoader]
        void Load()
        {
            AddInternal(new Sprite()
            {
                RelativeSizeAxes = Axes.Both,
                Texture = PixelTextureStore.Get("Resources/ControllerArrows"),
            });
        }
    }
}