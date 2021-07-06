using osu.Framework.Threading;

namespace qb.Animations
{
    public interface IClearLineAnimation
    {
        void Animation(int[] lines, Scheduler scheduler, GameScreen field);
    }
}