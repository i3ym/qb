using osu.Framework.Graphics;

namespace qb
{
    public class RandomGameController : Drawable
    {
        readonly GameScreen GameScreen;
        readonly Randomizer<QbKey> Randomizer = new Randomizer<QbKey>();
        QbKey? PreviousKey;

        public RandomGameController(GameScreen gamesc)
        {
            GameScreen = gamesc;
            gamesc.Controller.OnStartGame += Stop;

            Randomizer = new Randomizer<QbKey>()
                .AddRange(100, QbKey.Left, QbKey.Right, QbKey.Down)
                .AddRange(10, QbKey.A, QbKey.B);
        }


        public void Start()
        {
            Stop();

            GameScreen.Controller.RestartOnDeath = true;
            GameScreen.Controller.BeforeUpdate += Run;
        }
        public void Stop()
        {
            GameScreen.Controller.RestartOnDeath = false;
            GameScreen.Controller.BeforeUpdate -= Run;

            Release();
        }

        void Run()
        {
            Release();

            var key = Randomizer.Next();
            PreviousKey = key;
            GameScreen.Controller.TriggerPressed(key);
        }
        void Release()
        {
            if (PreviousKey is not null)
                GameScreen.Controller.TriggerReleased(PreviousKey.Value);
        }
    }
}