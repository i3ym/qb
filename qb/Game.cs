using System.Globalization;

namespace qb
{
    [Cached]
    public class Game : osu.Framework.Game
    {
        public bool IsTest { get; private set; }
        public Action OnAndroidPause = delegate { };
        public Action OnAndroidResume = delegate { };

        public Game(bool istest = false) => IsTest = istest;

        [BackgroundDependencyLoader]
        void Load(FrameworkConfigManager config)
        {
            Window.Title = "qb";

            config.SetValue(FrameworkSetting.ExecutionMode, ExecutionMode.SingleThread);
            if (string.IsNullOrEmpty(config.Get<string>(FrameworkSetting.Locale)))
            {
                var culture = CultureInfo.InstalledUICulture.Name;
                config.SetValue(FrameworkSetting.Locale, culture == "ru-RU" ? culture : "en-US");
            }

            Resources.AddStore(new DllResourceStore("qb.dll"));
            Textures.AddStore(new TextureStore(Host.Renderer, Host.CreateTextureLoaderStore(new DllResourceStore("qb"))));

            var pixelfontstore = new FontStore(Host.Renderer, scaleAdjust: 10, minFilterMode: TextureFilteringMode.Nearest);
            Fonts.AddStore(pixelfontstore);
            AddFont(Resources, "Resources/PixelFont", pixelfontstore);

            var pixeltexturestore = new TextureStore(Host.Renderer, Host.CreateTextureLoaderStore(Resources), true, TextureFilteringMode.Nearest, true, 1);
            Host.Dependencies.Cache(pixeltexturestore, new CacheInfo("PixelTextureStore"));

            var qconfig = new QbConfigManager(Host.Storage);
            Host.Dependencies.Cache(qconfig);

            Host.Dependencies.Cache(new SkinManager(Host.Storage, Resources, qconfig.GetBindable("skin", 0)));
            Host.Dependencies.Cache(new BackgroundController());


            var targetsize = new osuTK.Vector2(17, 21);
            var kb = new KeyboardController()
            {
                Child = new Container()
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new DrawSizePreservingFillContainer()
                        {
                            TargetDrawSize = targetsize,
                            Children = new Drawable[]
                            {
                                new Container()
                                {
                                    Anchor = Anchor.TopCentre,
                                    Origin = Anchor.TopCentre,
                                    Size = targetsize,
                                    Children = new Drawable[]
                                    {
                                        new GameScreen() { RelativeSizeAxes = Axes.Both }.With(Host.Dependencies.Cache),
                                        new PixelScreenStack().With(x => x.Push<MainMenu>()).With(Host.Dependencies.Cache),
                                    },
                                },
                                new Controller() { RelativeSizeAxes = Axes.Both, }.With(Host.Dependencies.Cache),
                            },
                        },
                        // new ControllerArrows() { Anchor = Anchor.BottomLeft, Origin = Anchor.BottomLeft, Size = new Vector2(1) },
                        // new ControllerButtons() { Anchor = Anchor.BottomRight, Origin = Anchor.BottomRight, Size = new Vector2(1) },
                    },
                }
            };

            Host.Dependencies.Cache(kb);
            Add(kb);
        }
    }
}