namespace qb.Gui.Menu
{
    public class GameOverMenu : ControllableMenuScreen, IKeyBindingHandler<QbKey>
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

        public override bool OnPressed(KeyBindingPressEvent<QbKey> e)
        {
            if (e.Action == QbKey.Back)
            {
                Back();
                return true;
            }

            return base.OnPressed(e);
        }
    }
}