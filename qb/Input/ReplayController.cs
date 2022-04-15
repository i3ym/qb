namespace qb.Input
{
    public class ReplayController : IController
    {
        public const string ReplayPath = "replay.qbr";

        public bool IsFinished { get; private set; } = false;

        readonly Replay Replay;
        ImmutableArray<ControllerState>.Enumerator Enumerator;

        public ReplayController(Replay replay)
        {
            Replay = replay;
            Restart();
        }

        public ControllerState GetState()
        {
            if (Enumerator.MoveNext())
                return Enumerator.Current;

            IsFinished = true;
            return default;
        }

        public void Restart()
        {
            IsFinished = false;
            Enumerator = Replay.Frames.GetEnumerator();
        }
    }
}