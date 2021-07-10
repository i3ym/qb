using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace qb.Gui.Menu
{
    public class SettingsMenu : MenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc, RandomGameController rndgamecontroller)
        {
            var buttons = new MenuButtonContainer()
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new[]
                {
                    new PixelButton(("Skin", "Скин"), Push<SkinChooseMenu>),
                    new PixelButton(("Language", "Язык"), Push<ChooseLanguageMenu>),
                    new PixelButton(("Back", "Назад"), Back),
                },
            };

            AddInternal(buttons);

            gamesc.Controller.IsPaused.Value = false;
            rndgamecontroller.Start();
        }
    }
}