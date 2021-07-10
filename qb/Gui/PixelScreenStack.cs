using osu.Framework.Allocation;
using osu.Framework.Screens;

namespace qb.Gui
{
    [Cached]
    public class PixelScreenStack : ScreenStack
    {
        public PixelScreenStack(bool suspendImmediately = true) : base(suspendImmediately) { }
        public PixelScreenStack(IScreen baseScreen, bool suspendImmediately = true) : base(baseScreen, suspendImmediately) { }
    }
}