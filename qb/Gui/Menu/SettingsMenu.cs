using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace qb.Gui.Menu
{
    public class SettingsMenu : MenuScreen
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
                    new PixelTextButton(("Skin", "Скин"), Push<SkinChooseMenu>),
                    new PixelTextButton(("Language", "Язык"), Push<ChooseLanguageMenu>),
                    new PixelTextButton(("Controls", "Управление"), Push<EditControlsMenu>),
                    new PixelTextButton(("Visuals", "Графика"), Push<EditGraphicsMenu>),
                    new PixelTextButton(("Back", "Назад"), Back),
                },
            };

            AddInternal(buttons);
        }
    }
}