namespace qb.Gui
{
    public partial class Controller : CompositeDrawable, IEditableDrawable
    {
        [Resolved] QbConfigManager ConfigManager { get; set; } = null!;
        [Resolved(name: "PixelTextureStore")] TextureStore TextureStore { get; set; } = null!;

        IEnumerable<Drawable> IEditableDrawable.EditableChildren => InternalChildren.OfType<ControllerButton<Sprite>>();

        public readonly Bindable<float> ChildrenScale = new(1);
        static readonly ImmutableDictionary<QbKey, ControllerButtonData> DefaultButtons;

        static Controller()
        {
            DefaultButtons = new Dictionary<QbKey, (Vector2, Anchor)>()
            {
                [QbKey.Up] = (new Vector2(0.059448004f, -3.5520165f), Anchor.BottomLeft),
                [QbKey.Down] = (new Vector2(7.0339622f, -0.029702783f), Anchor.BottomLeft),
                [QbKey.Left] = (new Vector2(0.053079598f, -0.029722711f), Anchor.BottomLeft),
                [QbKey.Right] = (new Vector2(3.535028f, -0.029724002f), Anchor.BottomLeft),

                [QbKey.A] = (new Vector2(-0.044585988f, -0.02972399f), Anchor.BottomRight),
                [QbKey.B] = (new Vector2(-3.5373669f, -0.044585988f), Anchor.BottomRight),
                [QbKey.Pause] = (new Vector2(-0.06486198f, -3.5819511f), Anchor.BottomRight),
            }.ToImmutableDictionary(k => k.Key, k => new ControllerButtonData(k.Value.Item1, 3.4494848f, k.Value.Item2, k.Value.Item2));
        }

        [BackgroundDependencyLoader]
        void Load()
        {
            addButton(QbKey.Up, "arrow", 0);
            addButton(QbKey.Down, "arrow", 180);
            addButton(QbKey.Left, "arrow", 270);
            addButton(QbKey.Right, "arrow", 90);

            addButton(QbKey.A, "A");
            addButton(QbKey.B, "B");
            addButton(QbKey.Pause, "Pause").With(x => x.Width = 2);

            ChildrenScale.Value = this.InternalChildren.OfType<ControllerButton<Sprite>>().First().Scale.X;


            ControllerButton<Sprite> addButton(QbKey key, string texture, float angle = 0) => createButton(key, texture, angle).With(AddInternal);
            ControllerButton<Sprite> createButton(QbKey key, string texture, float angle = 0)
            {
                var sprite = new Sprite()
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Texture = TextureStore.Get("Resources/" + texture),
                    Rotation = angle,
                };

                var btn = new ControllerButton<Sprite>(key, sprite);
                GetConfigBindable(key).Value.SetToDrawable(btn);
                ChildrenScale.BindValueChanged(e => btn.Scale = new(e.NewValue));

                return btn;
            }
        }

        Bindable<ControllerButtonData> GetConfigBindable(QbKey key) => ConfigManager.GetBindable<ControllerButtonData>($"keys.{key}", DefaultButtons[key]);

        public void ResetConfig()
        {
            ChildrenScale.Value = 1;
            foreach (var btn in this.InternalChildren.OfType<ControllerButton<Sprite>>())
                DefaultButtons[btn.Key].SetToDrawable(btn);
        }
        public void Save()
        {
            foreach (var child in this.InternalChildren.OfType<ControllerButton<Sprite>>())
                GetConfigBindable(child.Key).Value = ControllerButtonData.FromDrawable(child);
        }


        readonly record struct ControllerButtonData(Vector2 Position, float Scale, Anchor Anchor, Anchor Origin)
        {
            public static ControllerButtonData FromDrawable(Drawable target) => new ControllerButtonData(target.Position, target.Scale.X, target.Anchor, target.Origin);
            public void SetToDrawable(Drawable target)
            {
                target.Position = Position;
                target.Scale = new Vector2(Scale);
                target.Anchor = Anchor;
                target.Origin = Origin;
            }
        }
    }
}