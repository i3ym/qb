using System;

namespace qb.Animations
{
    public interface IGameOverAnimation
    {
        void Animation(GameScreen field, Action after);
    }
}