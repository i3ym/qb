namespace qb.Gui.Menu;

public partial class SettingsMenu : ControllableMenuScreen
{
    [BackgroundDependencyLoader]
    void Load()
    {
        AddButton(new PixelTextButton(("Skin", "Скин"), Push<SkinChooseMenu>));
        AddButton(new PixelTextButton(("Language", "Язык"), Push<ChooseLanguageMenu>));
        AddButton(new PixelTextButton(("Controls", "Управление"), Push<EditControlsMenu>));
        AddButton(new PixelTextButton(("Visuals", "Графика"), Push<EditGraphicsMenu>));
        AddButton(new PixelTextButton(("Back", "Назад"), Back));
    }
}
