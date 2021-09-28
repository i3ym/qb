using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;

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
                    new PixelTextButton(("Game over", "Вы проиграли")),
                    new PixelTextButton(),
                    new PixelTextButton(("Exit", "Выйти"), exit),
                },
            };

            AddInternal(buttons);


            void exit()
            {
                gamesc.Controller.StartGame();
                Push<MainMenu>();
            }
        }

        public override bool OnPressed(KeyBindingPressEvent<QbKey> e)
        {
            if (e.Action == QbKey.Back)
            {
                Back();
                return true;
            }

            return base.OnPressed(e);
        }
    }
}