using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using qb.Gui;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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

            var atlas = new qbTexture(Image.Load<Rgba32>(Textures.GetStream("Resources/Atlas.png")));
            var skin = new SkinManager(atlas);
            Host.Dependencies.Cache(skin);

            var gamesc = new GameScreen(10, 20);
            var controller = new FieldController(gamesc);
            Host.Dependencies.Cache(controller);

            var container = new DrawSizePreservingFillContainer() { TargetDrawSize = gamesc.Size };
            container.Add(gamesc);
            container.Add(controller);

            Add(container);

            Scheduler.AddDelayed(controller.StartGame, 100);

            // new MousePositionAbsoluteInput() { Position = new osuTK.Vector2() }.Apply(mgr.CurrentState, mgr);
        }
    }
}