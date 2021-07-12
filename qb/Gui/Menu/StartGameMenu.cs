using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using qb.Input;
using qb.Mods;

namespace qb.Gui.Menu
{
    public class StartGameMenu : MenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc, MultiController maincontroller)
        {
            var level = new PixelFlowContainer()
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                ChildAxes = Anchor.Centre,
                Children = new Drawable[]
                {
                    new PixelButton(("-"), leveldown) { Width = 2 },
                    PixelText.Create(gamesc.Controller.StartLevel, x => x.ToString("00")),
                    new PixelButton(("+"), levelup) { Width = 2 },
                },
            };
            var mods = new ToggleMenuButtonContainer<Mod>()
            {
                AutoSizeAxes = Axes.Both,
                ChildrenAnchor = Anchor.Centre,
                OnEnable = v => { v.OnAdd(gamesc); gamesc.Controller.Mods.Add(v); },
                OnDisable = v => { v.OnRemove(gamesc); gamesc.Controller.Mods.Remove(v); },
                Children = Mod.Mods.Select(x => new ValueMenuButton<Mod>(x, x.Name)).ToArray(),
            };

            var ffc = new PixelFlowContainer()
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                ChildAxes = Anchor.TopCentre,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new PixelButton(("Start", "Начать"), start),
                    level,
                    mods,
                    new PixelButton(("Back", "Назад"), Back),
                },
            };
            AddInternal(ffc);


            void start()
            {
                Hide();
                gamesc.Controller.Controller = maincontroller;
                gamesc.Controller.RestartOnDeath = false;

                gamesc.Controller.Recorder = new ReplayRecorder(); // TODO: <
                gamesc.Controller.OnGameOver += () => { System.IO.File.WriteAllText("/tmp/replay", Newtonsoft.Json.JsonConvert.SerializeObject(gamesc.Controller.Recorder.Frames)); };

                gamesc.Controller.StartGame();
            }
            void levelup() => gamesc.Controller.StartLevel.Value++;
            void leveldown() => gamesc.Controller.StartLevel.Value--;
        }
    }
}