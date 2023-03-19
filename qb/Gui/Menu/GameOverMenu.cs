namespace qb.Gui.Menu
{
    public partial class GameOverMenu : ControllableMenuScreen, IKeyBindingHandler<QbKey>
    {
        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc)
        {
            AddButton(new PixelTextButton(("Game over", "Вы проиграли")) { Focusable = false });
            AddButton(new PixelTextButton() { Focusable = false });
            AddButton(new PixelTextButton(("Exit", "Выйти"), exit));


            void exit()
            {
                gamesc.Controller.StartGame();
                Push<MainMenu>();
            }
        }
    }
}