namespace qb.Gui.Menu
{
    public partial class PauseMenu : ControllableMenuScreen, IKeyBindingHandler<QbKey>
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc, KeyboardController kcontroller)
        {
            AddButton(new PixelTextButton(("Continue", "Продолжить"), continueGame));
            AddButton(new PixelTextButton(("Settings", "Настройки"), Push<SettingsMenu>));
            AddButton(new PixelTextButton(("Exit", "Выйти"), Push<MainMenu>));


            void continueGame()
            {
                PixelScreenStack.PopTo(null);
                gamesc.Controller.IsPaused.Value = false;
            }
        }
    }
}