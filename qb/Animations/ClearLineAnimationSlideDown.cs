using System.Linq;
using osu.Framework.Graphics;

namespace qb.Animations
{
    public class ClearLineAnimationSlideDown : IClearLineAnimation
    {
        public void Animation(int[] lines, GameScreen gamesc)
        {
            const double animationTime = FieldController.MaxFps * FieldController.FrameInterval;

            var controller = gamesc.Controller;
            var gameObjects = gamesc.Playfield.GameObjects;

            var positions = gameObjects.Select(x => x.Y).ToArray();

            for (int y = 0; y < controller.MaxY; y++)
            {
                if (lines.Any(l => l == y))
                {
                    for (int x = 0; x < controller.MaxX; x++)
                    {
                        const int fadeouttime = 50;

                        gameObjects[x, y]
                            .MoveToY(1, animationTime, Easing.InExpo)
                            .Delay(1000 - fadeouttime)
                            .Then()
                                .FadeOut(fadeouttime);
                    }
                }
                else if (y > lines[0])
                {
                    int count = 0;
                    for (int i = 0; i < lines.Length; i++)
                        if (lines[i] < y) count++;
                        else break;

                    for (int x = 0; x < controller.MaxX; x++)
                        gameObjects[x, y].MoveToY(1f - (y - count + 1) / (float) controller.MaxY, animationTime, Easing.InExpo);
                }
            }

            gamesc.Controller.OnClearLineAnimationStop += reset;
            void reset()
            {
                gamesc.Controller.OnClearLineAnimationStop -= reset;

                for (int y = 0; y < controller.MaxY; y++)
                    for (int x = 0; x < controller.MaxX; x++)
                        gameObjects[x, y].FadeIn().MoveToY(positions[y + x * controller.MaxY]);
            }
        }
    }
}