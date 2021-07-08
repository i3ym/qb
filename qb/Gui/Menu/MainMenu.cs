using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;

namespace qb.Gui.Menu
{
    public class MainMenu : MenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(GameHost host, GameScreen gamesc, RandomGameController rndgamecontroller)
        {
            var buttons = new MenuButtonContainer()
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new[]
                {
                    new MenuButton(("Start", "Начать"), Push<StartGameMenu>),
                    new MenuButton(("Settings", "Настройки"), Push<SettingsMenu>),
                    new MenuButton(("Quit", "Выйти"), host.Exit),
                },
            };

            AddInternal(buttons);

            gamesc.Controller.StartGame();
            rndgamecontroller.Start();
        }
    }
}