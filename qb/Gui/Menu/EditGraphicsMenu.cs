namespace qb.Gui.Menu
{
    public class EditGraphicsMenu : MenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(QbConfigManager qconfig)
        {
            Background.Alpha = 0;

            var buttons = new MenuButtonContainer()
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new PixelSwitchButton(("Shadows", "Тени"), qconfig.GetBindable<bool>("shadows", true)),
                    new PixelSwitchButton(("Background", "Фон"), qconfig.GetBindable<bool>("showbg", true)),

                    new PixelTextButton(("Back", "Назад"), Back),
                },
            };

            AddInternal(buttons);
        }
    }
}