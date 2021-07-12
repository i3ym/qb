using System.Collections.Generic;

namespace qb
{
    public class ReplayRecorder
    {
        public IReadOnlyList<ControllerState> Frames => _Frames;
        readonly List<ControllerState> _Frames = new();

        public void Record(ControllerState state) => _Frames.Add(state);
    }
}