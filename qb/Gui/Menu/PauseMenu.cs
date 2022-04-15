namespace qb.Gui.Menu
{
    public class PauseMenu : MenuScreen, IKeyBindingHandler<QbKey>
    {
        [Resolved] GameScreen GameScreen { get; set; } = null!;

        [BackgroundDependencyLoader]
        void Load()
        {
            var buttons = new MenuButtonContainer()
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new[]
                {
                    new PixelTextButton(("Continue", "Продолжить"), ContinueGame),
                    new PixelTextButton(("Settings", "Настройки"), Push<SettingsMenu>),
                    new PixelTextButton(("Exit", "Выйти"), Push<MainMenu>),
                },
            };

            AddInternal(buttons);
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