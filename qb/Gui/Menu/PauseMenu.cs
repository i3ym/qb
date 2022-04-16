namespace qb.Gui.Menu
{
    public class PauseMenu : ControllableMenuScreen, IKeyBindingHandler<QbKey>
    {
        [Resolved] GameScreen GameScreen { get; set; } = null!;

        [BackgroundDependencyLoader]
        void Load()
        {
            AddButton(new PixelTextButton(("Continue", "Продолжить"), ContinueGame));
            AddButton(new PixelTextButton(("Settings", "Настройки"), Push<SettingsMenu>));
            AddButton(new PixelTextButton(("Exit", "Выйти"), Push<MainMenu>));
        }
        void ContinueGame()
        {
            PixelScreenStack.PopTo(null);
            GameScreen.Controller.IsPaused.Value = false;
        }

        public override bool OnPressed(KeyBindingPressEvent<QbKey> e)
        {
            if (e.Action == QbKey.Back)
            {
                ContinueGame();
                return true;
            }

            return base.OnPressed(e);
        }
    }
}