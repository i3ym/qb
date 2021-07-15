using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Screens;
using qb.Input;

namespace qb.Gui.Menu
{
    public class MainMenu : MenuScreen
    {
        [Resolved] GameScreen GameScreen { get; set; } = null!;
        [Resolved] BackgroundController BackgroundGameController { get; set; } = null!;
        [Resolved] Storage Storage { get; set; } = null!;

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
            ResumeAutoGame();
            base.OnResuming(last);
        }
        public override void OnEntering(IScreen last)
        {
            ResumeAutoGame();
            base.OnEntering(last);
        }

        void ResumeAutoGame()
        {
            GameScreen.Controller.IsPaused.Value = false;
            GameScreen.Controller.RestartOnDeath = true;
            GameScreen.Controller.Controller = BackgroundGameController;

            try
            {
                if (Storage.Exists(ReplayController.ReplayPath))
                    BackgroundGameController.Controller = new ReplayController(Replay.FromJson(Storage.GetStream(ReplayController.ReplayPath)));
            }
            catch { }
        }
    }
}