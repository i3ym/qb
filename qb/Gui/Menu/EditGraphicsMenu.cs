namespace qb.Gui.Menu
{
    public class EditGraphicsMenu : ControllableMenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(QbConfigManager qconfig)
        {
            Background.Alpha = 0;

            AddButton(new PixelSwitchButton(("Shadows", "Тени"), qconfig.GetBindable<bool>("shadows", true)));
            AddButton(new PixelSwitchButton(("Background", "Фон"), qconfig.GetBindable<bool>("showbg", true)));
            AddButton(new PixelTextButton(("Back", "Назад"), Back));
        }
    }
}