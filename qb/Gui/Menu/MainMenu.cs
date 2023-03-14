namespace qb.Gui.Menu
{
    public partial class MainMenu : ControllableMenuScreen
    {
        [Resolved] GameScreen GameScreen { get; set; } = null!;
        [Resolved] BackgroundController BackgroundGameController { get; set; } = null!;
        [Resolved] Storage Storage { get; set; } = null!;

        [BackgroundDependencyLoader]
        void Load(GameHost host)
        {
            AddButton(new PixelTextButton(("Start", "Начать"), Push<StartGameMenu>));
            AddButton(new PixelTextButton(("Settings", "Настройки"), Push<SettingsMenu>));
            AddButton(new PixelTextButton(("Quit", "Выйти"), host.CanExit ? host.Exit : () => host.SuspendToBackground()));
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