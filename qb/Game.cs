using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osuTK.Graphics.ES30;
using qb.Gui;

namespace qb
{
    public class Game : osu.Framework.Game
    {
        [BackgroundDependencyLoader]
        void Load(FrameworkConfigManager config)
        {
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


            var menu = new MainMenu() { RelativeSizeAxes = Axes.Both };

            var gamesc = new GameScreen(10, 20) { RelativeSizeAxes = Axes.Both, Alpha = 0 };
            var controller = new FieldController();
            Host.Dependencies.Cache(gamesc);
            Host.Dependencies.Cache(controller);

            var targetsize = new osuTK.Vector2(gamesc.FieldWidth + 7, gamesc.FieldHeight + 1);
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
                        controller,

                        menu,
                    },
                },
            };

            Add(container);

            // new MousePositionAbsoluteInput() { Position = new osuTK.Vector2() }.Apply(mgr.CurrentState, mgr);
        }
    }
}