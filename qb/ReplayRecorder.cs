using System.Collections.Generic;
using System.Collections.Immutable;

namespace qb
{
    public class ReplayRecorder
    {
        readonly int Seed;
        readonly List<ControllerState> Frames = new List<ControllerState>();

        public ReplayRecorder(int seed) => Seed = seed;

        public void Record(ControllerState state) => Frames.Add(state);
        public Replay AsReplay() => new Replay(Seed, Frames.ToImmutableArray());
    }
}