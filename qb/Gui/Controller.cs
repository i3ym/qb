using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK;

namespace qb.Gui
{
    public class Controller : CompositeDrawable
    {
        [Resolved] QbConfigManager ConfigManager { get; set; } = null!;
        [Resolved(name: "PixelTextureStore")] TextureStore TextureStore { get; set; } = null!;

        public bool EditMode
        {
            get => _EditMode;
            set
            {
                _EditMode = value;

                if (!value)
                {
                    foreach (var child in InternalChildren.OfType<IControllerButton>())
                    {
                        var dc = (Drawable) child;
                        var data = new ControllerButtonData(dc.Position, dc.Scale.X, dc.Anchor, dc.Origin);

                        ConfigManager.Set("keys." + child.Key, data);
                    }
                }
            }
        }
        bool _EditMode;

        public float ChildrenScale
        {
            get => ((Drawable) InternalChildren.OfType<IHandleable>().First()).Scale.X;
            set
            {
                foreach (var child in InternalChildren.OfType<IHandleable>())
                    ((Drawable) child).Scale = new Vector2(value);
            }
        }

        [BackgroundDependencyLoader]
        void Load()
        {
            AddInternal(createArrow(QbKey.Up, 0, new Vector2(0, 0), Anchor.BottomLeft));
            AddInternal(createArrow(QbKey.Down, 180, new Vector2(1, 0), Anchor.BottomLeft));
            AddInternal(createArrow(QbKey.Left, 270, new Vector2(2, 0), Anchor.BottomLeft));
            AddInternal(createArrow(QbKey.Right, 90, new Vector2(3, 0), Anchor.BottomLeft));

            AddInternal(createText(QbKey.A, "A", new Vector2(0, 0), Anchor.BottomRight));
            AddInternal(createText(QbKey.B, "B", new Vector2(-1.1f, 0), Anchor.BottomRight));
            AddInternal(createText(QbKey.Back, ("Back", "Назад"), new Vector2(-.05f, -1.1f), Anchor.BottomRight, fontSize: .4f).With(x => x.Width = 2));

            EditMode = false;



            ControllerButtonData getCfg(QbKey key, Vector2 defaultPos, Anchor defaultAnchor) => ConfigManager.Get("keys." + key, new ControllerButtonData(defaultPos, 1, defaultAnchor));
            ControllerButton<Sprite> createArrow(QbKey key, int angle, Vector2 defaultPos, Anchor defaultAnchor)
            {
                var cfg = getCfg(key, defaultPos, defaultAnchor);
                return new ControllerButton<Sprite>(key,
                    new Sprite() { RelativeSizeAxes = Axes.Both, Anchor = Anchor.Centre, Origin = Anchor.Centre, Rotation = angle, Texture = TextureStore.Get("Resources/ControllerArrow") })
                {
                    Position = cfg.Position,
                    Scale = new Vector2(cfg.Scale),
                    Anchor = cfg.Anchor,
                    Origin = cfg.Origin,
                    Size = new osuTK.Vector2(1),
                    HandleEvents = false,
                };
            }
            ControllerButton<PixelText> createText(QbKey key, TranslatedString text, Vector2 defaultPos, Anchor defaultAnchor, float fontSize = .9f)
            {
                var cfg = getCfg(key, defaultPos, defaultAnchor);
                return new ControllerButton<PixelText>(key,
                    new PixelText(text) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Colour = Colour4.Black })
                {
                    Position = cfg.Position,
                    Scale = new Vector2(cfg.Scale),
                    Anchor = cfg.Anchor,
                    Origin = cfg.Origin,
                    Background = Colour4.White,
                    Size = new osuTK.Vector2(1),
                    HandleEvents = false,
                }.With(x => x.Child.Font = x.Child.Font.With(size: fontSize));
            }
        }

        Drawable? CurrentlyMoving;
        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (EditMode)
            {
                foreach (var child in InternalChildren)
                    if (child.IsHovered)
                    {
                        CurrentlyMoving = child;
                        break;
                    }
            }

            return base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseUpEvent e)
        {
            CurrentlyMoving = null;
            base.OnMouseUp(e);
        }
        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (CurrentlyMoving is { })
            {
                CurrentlyMoving.Position += e.Delta;
            }

            return base.OnMouseMove(e);
        }


        public readonly struct ControllerButtonData
        {
            public readonly Vector2 Position;
            public readonly float Scale;
            public readonly Anchor Anchor, Origin;

            public ControllerButtonData(Vector2 position, float scale, Anchor anchor) : this(position, scale, anchor, anchor) { }

            [JsonConstructor]
            public ControllerButtonData(Vector2 position, float scale, Anchor anchor, Anchor origin)
            {
                Position = position;
                Scale = scale;
                Anchor = anchor;
                Origin = origin;
            }
        }
    }
}