using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using qb.Mods;

namespace qb.Gui.Menu
{
    public class StartGameMenu : MenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc)
        {
            var level = new PixelFlowContainer()
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Children = new Drawable[]
                {
                    new MenuButton(("<"), levelup),
                    new PixelText().With(x => gamesc.Controller.StartLevel.BindValueChanged(e => x.Text = e.NewValue.ToString(), true)),
                    new MenuButton((">"), leveldown),
                },
            };
            var mods = new ToggleMenuButtonContainer<Mod>()
            {
                RelativeSizeAxes = Axes.X,
                Height = 16,
                OnEnable = v => gamesc.Controller.Mods.Add(v),
                OnDisable = v => gamesc.Controller.Mods.Remove(v),
                Children = Mod.Mods.Select(x => new ToggleMenuButton<Mod>(x, x.Name)).ToArray(),
            };

            var ffc = new PixelFlowContainer()
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new MenuButton(("Start", "Начать"), start) { Origin = Anchor.TopCentre, Y = 1 },
                    mods,
                    level,
                    new MenuButton(("Back", "Назад"), Back) { Origin = Anchor.BottomCentre, Y = -1 },
                },
            };
            AddInternal(ffc);


            void start()
            {
                Hide();
                gamesc.Controller.StartGame();
            }
            void levelup() => gamesc.Controller.StartLevel.Value++;
            void leveldown() => gamesc.Controller.StartLevel.Value--;
        }
    }
}