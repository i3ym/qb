using System.Collections.Generic;
using System.Collections.Immutable;

namespace qb.Input
{
    public class ReplayController : IController
    {
        public bool IsFinished { get; private set; } = false;

        readonly ImmutableArray<ControllerState> Frames;
        ImmutableArray<ControllerState>.Enumerator Enumerator;

        public ReplayController(IEnumerable<ControllerState> frames)
        {
            Frames = frames.ToImmutableArray();
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
            Enumerator = Frames.GetEnumerator();
        }
    }
}