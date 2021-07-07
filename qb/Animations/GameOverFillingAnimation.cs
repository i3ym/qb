using System;
using System.Collections.Generic;
using osu.Framework.Threading;

namespace qb.Animations
{
    public class GameOverFillingAnimation : IGameOverAnimation
    {
        public void Animation(Scheduler scheduler, GameScreen field)
        {
            var enumerator = animation();

            ScheduledDelegate deleg = null!;
            Action action = () =>
            {
                if (!enumerator.MoveNext())
                    deleg.Cancel();
            };

            deleg = new ScheduledDelegate(action, repeatInterval: 1_000 / 60) { PerformRepeatCatchUpExecutions = false };
            scheduler.Add(deleg);


            IEnumerator<bool> animation()
            {
                var controller = field.Controller;

                for (int y = controller.Tiles.Height - 1; y >= controller.Tiles.Height / 2; y--)
                    for (int x = 0; x < controller.Tiles.Width; x++)
                    {
                        field.Playfield[x, y] = CubeTile.Background;
                        field.Playfield[controller.Tiles.Width - 1 - x, controller.Tiles.Height - 1 - y] = CubeTile.Background;

                        yield return true;
                    }
            }
        }
    }
}