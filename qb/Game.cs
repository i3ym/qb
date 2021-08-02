using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics.ES30;
using qb.Gui;
using qb.Gui.Menu;
using qb.Input;

namespace qb
{
    public class Game : osu.Framework.Game
    {
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
            Textures.AddStore(new TextureStore(Host.CreateTextureLoaderStore(new DllResourceStore("qb"))));

            var pixelfontstore = new FontStore(scaleAdjust: 5, minFilterMode: osuTK.Graphics.ES30.All.Nearest);
            Fonts.AddStore(pixelfontstore);
            AddFont(Resources, "Resources/PixelFont", pixelfontstore);

            var pixeltexturestore = new TextureStore(Textures, true, All.Nearest, true, 1);
            Textures.AddStore(pixeltexturestore);
            Host.Dependencies.Cache(pixeltexturestore, new CacheInfo("PixelTextureStore"));

            var atlasStore = new TextureStore(Textures, false, All.Nearest, true, 1);
            Textures.AddStore(atlasStore);

            var qconfig = new QbConfigManager(Host.Storage);
            Host.Dependencies.Cache(qconfig);

            Host.Dependencies.Cache(new SkinManager(atlasStore.Get("Resources/Atlas"), qconfig.GetBindable<int>(QbConfigField.SelectedSkin)));
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
                            },
                        },
                        new ControllerArrows() { Anchor = Anchor.BottomLeft, Origin = Anchor.BottomLeft, Size = new Vector2(1) },
                        new ControllerButtons() { Anchor = Anchor.BottomRight, Origin = Anchor.BottomRight, Size = new Vector2(1) },
                    },
                }
            };

            Host.Dependencies.Cache(kb);
            Add(kb);
        }
    }
}