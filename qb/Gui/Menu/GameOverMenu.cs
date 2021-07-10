using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace qb.Gui.Menu
{
    public class GameOverMenu : MenuScreen
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
                    new PixelButton(("Exit", "Выйти"), BackToRoot),
                },
            };

            AddInternal(buttons);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
                Back();

            return base.OnKeyDown(e);
        }
    }
}