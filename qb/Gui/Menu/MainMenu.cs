using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
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


        public override void OnShow(MenuScreen? last)
        {
            ResumeAutoGame();
            base.OnShow(last);
        }

        void ResumeAutoGame()
        {
            GameScreen.Controller.RestartOnDeath = true;

            try
            {
                if (Storage.Exists(ReplayController.ReplayPath))
                {
                    var replay = Replay.Deserialize(Storage.GetStream(ReplayController.ReplayPath));
                    // replay = Newtonsoft.Json.JsonConvert.DeserializeObject<Replay>(new System.IO.StreamReader( Storage.GetStream(ReplayController.ReplayPath+"j")).ReadToEnd());

                    GameScreen.Controller.Controller = new ReplayController(replay);
                    GameScreen.Controller.StartGame(seed: replay.Seed);

                    return;
                }
            }
            catch { }

            GameScreen.Controller.IsPaused.Value = false;
            GameScreen.Controller.Controller = BackgroundGameController;
        }
    }
}