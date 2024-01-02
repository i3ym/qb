namespace qb.Gui.Menu;

public partial class GameOverMenu : ControllableMenuScreen, IKeyBindingHandler<QbKey>
{
    [BackgroundDependencyLoader]
    void Load(GameScreen gamesc, KeyboardController maincontroller, Storage storage)
    {
        AddButton(new PixelTextButton(("Game over", "Вы проиграли")) { Focusable = false });
        AddButton(new PixelTextButton() { Focusable = false });
        AddButton(new PixelTextButton(("Restart", "Начать заново"), restart));
        AddButton(new PixelTextButton(("Exit", "Выйти"), exit));


        void restart()
        {
            PixelScreenStack.PopTo(null);
            gamesc.Controller.StartGame(maincontroller, storage);
        }
        void exit()
        {
            gamesc.Controller.StartGame();
            Push<MainMenu>();
        }
    }
}
