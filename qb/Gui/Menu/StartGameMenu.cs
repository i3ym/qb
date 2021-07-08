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
                ChildAxes = Anchor.TopCentre,
                Children = new Drawable[]
                {
                    new MenuButton(("-"), leveldown),
                    new PixelText().With(x => gamesc.Controller.StartLevel.BindValueChanged(e => x.Text = e.NewValue.ToString(), true)),
                    new MenuButton(("+"), levelup),
                },
            };
            var mods = new ToggleMenuButtonContainer<Mod>()
            {
                AutoSizeAxes = Axes.Both,
                ChildAxes = Anchor.Centre,
                OnEnable = v => gamesc.Controller.Mods.Add(v),
                OnDisable = v => gamesc.Controller.Mods.Remove(v),
                Children = Mod.Mods.Select(x => new ToggleMenuButton<Mod>(x, x.Name)).ToArray(),
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
                    new MenuButton(("Start", "Начать"), start),
                    level,
                    mods,
                    new MenuButton(("Back", "Назад"), Back),
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