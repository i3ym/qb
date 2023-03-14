namespace qb.Gui.Menu
{
    public partial class ChooseLanguageMenu : ChooseMenu
    {
        protected override TranslatedString Title => ("Language", "Язык");

        [BackgroundDependencyLoader]
        void Load(FrameworkConfigManager config)
        {
            var langbindable = config.GetBindable<string>(FrameworkSetting.Locale);

            AddButtons(
                new ChooseMenuButton<string>("ru-RU", langbindable, "Русский"),
                new ChooseMenuButton<string>("en-US", langbindable, "English")
            );
        }
    }
}