using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK.Input;

namespace qb.Gui.Menu
{
    public class PauseMenu : MenuScreen
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
                    new MenuButton(("Continue", "Продолжить"), ContinueGame),
                    new MenuButton(("Settings", "Настройки"), Push<SettingsMenu>),
                    new MenuButton(("Exit", "Выйти"), Push<MainMenu>),
                },
            };

            AddInternal(buttons);
        }
        void ContinueGame()
        {
            Hide();
            GameScreen.Controller.IsPaused.Value = false;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
                ContinueGame();

            return base.OnKeyDown(e);
        }
    }
}