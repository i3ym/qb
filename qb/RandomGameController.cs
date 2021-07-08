using osu.Framework.Graphics;
using osu.Framework.Input.StateChanges;
using osuTK.Input;

namespace qb
{
    public class RandomGameController : Drawable
    {
        readonly GameScreen GameScreen;
        readonly Randomizer<Key> Randomizer = new Randomizer<Key>();
        KeyboardKeyInput? PreviousInput;

        public RandomGameController(GameScreen gamesc)
        {
            GameScreen = gamesc;
            gamesc.Controller.OnStartGame += Stop;

            Randomizer = new Randomizer<Key>()
                .AddRange(100, Key.Left, Key.Right, Key.Down)
                .AddRange(10, Key.Q, Key.E);
        }


        public void Start()
        {
            GameScreen.InputManager.UseParentInput = false;
            GameScreen.Controller.RestartOnDeath = true;
            GameScreen.Controller.BeforeUpdate += Run;
        }
        public void Stop()
        {
            GameScreen.InputManager.UseParentInput = true;
            GameScreen.Controller.RestartOnDeath = false;
            GameScreen.Controller.BeforeUpdate -= Run;
        }

        void Run()
        {
            try
            {
                if (PreviousInput is not null)
                    new KeyboardKeyInput(PreviousInput.Entries[0].Button, false).Apply(GameScreen.InputManager.CurrentState, GameScreen.InputManager);

                PreviousInput = new KeyboardKeyInput(Randomizer.Next(), true);
                PreviousInput.Apply(GameScreen.InputManager.CurrentState, GameScreen.InputManager);
            }
            catch { }
        }
    }
}