using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace qb.Gui
{
    public class ControllerArrows : CompositeDrawable, IController
    {
        [Resolved(name: nameof(PixelTextureStore))] TextureStore PixelTextureStore { get; init; } = null!;

        readonly ControllerButton Up, Down, Left, Right;

        public ControllerArrows()
        {
            Up = create(Anchor.TopCentre);
            Down = create(Anchor.BottomCentre);
            Left = create(Anchor.CentreLeft);
            Right = create(Anchor.CentreRight);

            ControllerButton create(Anchor anchor) => new ControllerButton(anchor) { RelativeSizeAxes = Axes.Both, Size = new(1 / 3f) };
        }

        [BackgroundDependencyLoader]
        void Load()
        {
            AddInternal(new Sprite()
            {
                RelativeSizeAxes = Axes.Both,
                Texture = PixelTextureStore.Get("Resources/ControllerArrows"),
            });

            AddInternal(Up);
            AddInternal(Down);
            AddInternal(Left);
            AddInternal(Right);
        }

        public ControllerState GetState() =>
            new ControllerState()
            {
                Up = Up.IsPressed,
                Down = Down.IsPressed,
                Left = Left.IsPressed,
                Right = Right.IsPressed,
            };
    }
}