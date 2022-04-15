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
            AddInternal(createButton(QbKey.Up, "arrow", new Vector2(0, 0), Anchor.BottomLeft, 0));
            AddInternal(createButton(QbKey.Down, "arrow", new Vector2(1, 0), Anchor.BottomLeft, 180));
            AddInternal(createButton(QbKey.Left, "arrow", new Vector2(2, 0), Anchor.BottomLeft, 270));
            AddInternal(createButton(QbKey.Right, "arrow", new Vector2(3, 0), Anchor.BottomLeft, 90));

            AddInternal(createButton(QbKey.A, "A", new Vector2(0, 0), Anchor.BottomRight));
            AddInternal(createButton(QbKey.B, "B", new Vector2(-1.1f, 0), Anchor.BottomRight));
            AddInternal(createButton(QbKey.Back, "Pause", new Vector2(-.05f, -1.1f), Anchor.BottomRight).With(x => x.Width = 2));

            EditMode = false;



            ControllerButtonData getCfg(QbKey key, Vector2 defaultPos, Anchor defaultAnchor) => ConfigManager.Get("keys." + key, new ControllerButtonData(defaultPos, 1, defaultAnchor));
            ControllerButton<Sprite> createButton(QbKey key, string texture, Vector2 defaultPos, Anchor defaultAnchor, float angle = 0)
            {
                var sprite = new Sprite()
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Texture = TextureStore.Get("Resources/" + texture),
                    Rotation = angle,
                };

                var cfg = getCfg(key, defaultPos, defaultAnchor);
                return new ControllerButton<Sprite>(key, sprite)
                {
                    Position = cfg.Position,
                    Scale = new Vector2(cfg.Scale),
                    Anchor = cfg.Anchor,
                    Origin = cfg.Origin,
                    Size = new osuTK.Vector2(1),
                    HandleEvents = false,
                };
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

            [Newtonsoft.Json.JsonConstructor]
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