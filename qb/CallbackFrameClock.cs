using System;
using osu.Framework.Timing;

namespace qb
{
    public class CallbackFrameClock : IFrameBasedClock
    {
        public event Action Process = delegate { };

        public double ElapsedFrameTime => throw new System.NotImplementedException();
        public double FramesPerSecond => throw new System.NotImplementedException();
        public FrameTimeInfo TimeInfo => throw new System.NotImplementedException();
        public double CurrentTime => throw new System.NotImplementedException();
        public double Rate => throw new System.NotImplementedException();
        public bool IsRunning => true;

        public double MaxFps;

        public void ProcessFrame()
        {
            
        }
    }
}