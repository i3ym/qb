using osu.Framework.Threading;

namespace qb.Animations
{
    public interface IGameOverAnimation
    {
        void Animation(Scheduler scheduler, GameScreen field);
    }
}