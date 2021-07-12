using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;

namespace qb.Gui.Menu
{
    public class PauseMenu : MenuScreen, IKeyBindingHandler<QbKey>
    {
        [Resolved] GameScreen GameScreen { get; init; } = null!;

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
                    new PixelButton(("Continue", "Продолжить"), ContinueGame),
                    new PixelButton(("Settings", "Настройки"), Push<SettingsMenu>),
                    new PixelButton(("Exit", "Выйти"), Push<MainMenu>),
                },
            };

            AddInternal(buttons);
        }
        void ContinueGame()
        {
            Hide();
            GameScreen.Controller.IsPaused.Value = false;
        }

        public bool OnPressed(QbKey action)
        {
            if (action == QbKey.Back)
            {
                ContinueGame();
                return true;
            }

            return false;
        }
        public void OnReleased(QbKey action) { }
    }
}