namespace qb.Gui.Menu
{
    public class SkinChooseMenu : ChooseMenu
    {
        protected override TranslatedString Title => ("Skin", "Скин");

        [BackgroundDependencyLoader]
        void Load(SkinManager skinmgr) => AddButtons(skinmgr.Skins.Select(s => new ChooseMenuButton<Skin>(s, skinmgr.SelectedSkin, s.Name)));
    }
}