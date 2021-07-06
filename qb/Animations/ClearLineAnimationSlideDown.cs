using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Threading;
using osuTK;

namespace qb.Animations
{
    public class ClearLineAnimationSlideDown : IClearLineAnimation
    {
        public void Animation(int[] lines, Scheduler scheduler, GameScreen field)
        {
            const int animationTime = 1000;

            var controller = field.Controller;
            var gameObjects = field.Playfield.GameObjects;

            scheduler.Add(() =>
            {
                for (int yy = 0; yy < controller.MaxY; yy++)
                {
                    if (lines.Any(y => y == yy))
                    {
                        for (int xx = 0; xx < controller.MaxX; xx++)
                            gameObjects[xx, yy].MoveTo(new Vector2(xx, controller.MaxY), animationTime, Easing.OutQuad);
                    }
                    else if (yy > lines[0])
                    {
                        int count = 0;
                        for (int i = 0; i < lines.Length; i++)
                            if (lines[i] < yy) count++;
                            else break;

                        for (int xx = 0; xx < controller.MaxX; xx++)
                            gameObjects[xx, yy].MoveTo(new Vector2(xx, count - yy - 1 + controller.MaxY), animationTime, Easing.OutQuad);
                    }
                }
            });

            scheduler.AddDelayed(() =>
            {
                for (int yy = 0; yy < controller.MaxY; yy++)
                    for (int xx = 0; xx < controller.MaxX; xx++)
                        gameObjects[xx, yy].MoveTo(new Vector2(xx, controller.MaxY - 1 - yy));
            }, animationTime);
        }
    }
}