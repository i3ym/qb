using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;

namespace qb.Gui
{
    public class MainMenu : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc, GameHost host)
        {
            var buttons = new MenuButtonContainer()
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new MenuButton()
                    {
                        TranslatedText = ("Start", "Начать"),
                        Action = start,
                    },

                    new MenuButton()
                    {
                        TranslatedText = ("Quit", "Выйти"),
                        Action = host.Exit,
                    },
                },
            };

            AddInternal(buttons);


            void start()
            {
                gamesc.Show();
                Hide();
                gamesc.Controller.StartGame();
            }
        }
    }
}