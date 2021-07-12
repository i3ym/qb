using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using qb.Input;

namespace qb.Gui
{
    public class ControllerArrows : CompositeDrawable, IController
    {
        [Resolved(name: nameof(PixelTextureStore))] TextureStore PixelTextureStore { get; set; } = null!;

        readonly ControllerButton Up, Down, Left, Right;

        public ControllerArrows()
        {
            Up = create(Anchor.TopCentre);
            Down = create(Anchor.BottomCentre);
            Left = create(Anchor.CentreLeft);
            Right = create(Anchor.CentreRight);

            ControllerButton create(Anchor anchor) => new ControllerButton(anchor) { RelativeSizeAxes = Axes.Both, Size = new osuTK.Vector2(1 / 3f) };
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
            new ControllerState
            (
                up: Up.IsPressed,
                down: Down.IsPressed,
                left: Left.IsPressed,
                right: Right.IsPressed
            );
    }
}