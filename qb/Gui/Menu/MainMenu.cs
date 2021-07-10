using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using qb.Input;

namespace qb.Gui.Menu
{
    public class MainMenu : MenuScreen
    {
        [Resolved] GameScreen GameScreen { get; init; } = null!;
        [Resolved] RandomGameController RandomGameController { get; init; } = null!;

        [BackgroundDependencyLoader]
        void Load(GameHost host)
        {
            var buttons = new MenuButtonContainer()
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new[]
                {
                    new PixelButton(("Start", "Начать"), Push<StartGameMenu>),
                    new PixelButton(("Settings", "Настройки"), Push<SettingsMenu>),
                    new PixelButton(("Quit", "Выйти"), host.Exit),
                },
            };

            AddInternal(buttons);
        }


        public override void OnResuming(IScreen last)
        {
            GameScreen.Controller.IsPaused.Value = false;
            GameScreen.Controller.Controller = RandomGameController;

            base.OnResuming(last);
        }
        public override void OnEntering(IScreen last)
        {
            GameScreen.Controller.IsPaused.Value = false;
            GameScreen.Controller.Controller = RandomGameController;

            base.OnEntering(last);
        }
    }
}