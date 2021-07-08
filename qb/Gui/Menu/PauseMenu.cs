using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace qb.Gui.Menu
{
    public class PauseMenu : MenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc)
        {
            var buttons = new MenuButtonContainer()
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new[]
                {
                    new MenuButton(("Continue", "Продолжить"), continueg),
                    new MenuButton(("Settings", "Настройки"), Push<SettingsMenu>),
                    new MenuButton(("Exit", "Выйти"), exit),
                },
            };

            AddInternal(buttons);


            void continueg()
            {
                Hide();
                gamesc.Controller.IsPaused.Value = false;
            }
            void exit()
            {
                gamesc.Controller.Reset();
                this.Push(new MainMenu());
            }
        }
    }
}