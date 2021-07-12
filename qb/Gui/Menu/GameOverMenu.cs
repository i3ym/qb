using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;

namespace qb.Gui.Menu
{
    public class GameOverMenu : MenuScreen, IKeyBindingHandler<QbKey>
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc)
        {
            var buttons = new MenuButtonContainer()
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new[]
                {
                    new PixelButton(("Game over", "Вы проиграли")),
                    new PixelButton(),
                    new PixelButton(("Exit", "Выйти"), exit),
                },
            };

            AddInternal(buttons);


            void exit()
            {
                gamesc.Controller.StartGame();
                BackToRoot();
            }
        }

        public bool OnPressed(QbKey action)
        {
            if (action == QbKey.Back)
            {
                Back();
                return true;
            }

            return false;
        }
        public void OnReleased(QbKey action) { }
    }
}