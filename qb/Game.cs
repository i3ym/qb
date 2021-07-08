using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osuTK.Graphics.ES30;
using qb.Gui;
using qb.Gui.Menu;

namespace qb
{
    public class Game : osu.Framework.Game
    {
        [BackgroundDependencyLoader]
        void Load(FrameworkConfigManager config)
        {
            Window.Title = "qb";

            Resources.AddStore(new DllResourceStore("qb.dll"));
            Textures.AddStore(new TextureStore(Host.CreateTextureLoaderStore(new DllResourceStore("qb"))));

            var pixelfontstore = new FontStore(scaleAdjust: 5, minFilterMode: osuTK.Graphics.ES30.All.Nearest);
            Fonts.AddStore(pixelfontstore);
            AddFont(Resources, "Resources/PixelFont", pixelfontstore);

            var qconfig = new QbConfigManager(Host.Storage);
            Host.Dependencies.Cache(qconfig);

            var atlasStore = new TextureStore(Textures, false, All.Nearest, true, 1);
            Textures.AddStore(atlasStore);

            var atlas = atlasStore.Get("Resources/Atlas");
            var skin = new SkinManager(atlas, qconfig.GetBindable<int>(QbConfigField.SelectedSkin));
            Host.Dependencies.Cache(skin);



            var gamesc = new GameScreen() { RelativeSizeAxes = Axes.Both };
            Host.Dependencies.Cache(gamesc);

            var rndgc = new RandomGameController(gamesc);
            Host.Dependencies.Cache(rndgc);

            var targetsize = new osuTK.Vector2(17, 21);
            var container = new DrawSizePreservingFillContainer()
            {
                TargetDrawSize = targetsize,
                Child = new Container()
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = targetsize,
                    Children = new Drawable[]
                    {
                        gamesc,
                        new PixelScreenStack(new MainMenu()).With(Host.Dependencies.Cache),
                    },
                },
            };
            Add(container);
        }
    }
}