using osu.Framework.Allocation;
using osu.Framework.Configuration;

namespace qb.Gui.Menu
{
    public class ChooseLanguageMenu : ChooseMenu
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