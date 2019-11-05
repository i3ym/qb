using System.Collections.Generic;
using System.Linq;
using qb.Shared;
using qb.Shared.Animations;
using qb.Shared.Mods;
using UnityEngine;

namespace qb.Unity
{
    public class RemoveLinesAnimationSlideDown : RemoveLinesAnimation<UnityGameScreen>
    {
        protected override IEnumerator<bool> Animation(int[] lines, FieldController controller, UnityGameScreen field)
        {
            float speed = .1f;
            float time = Time.time + 1f;
            int count;
            var gameObjects = field.FieldGrid.GameObjects;

            while (time > Time.time)
            {
                speed += .001f;
                yield return true;

                for (int yy = 0; yy < controller.MaxY; yy++)
                {
                    if (lines.Any(y => y == yy))
                    {
                        for (int xx = 0; xx < controller.MaxX; xx++)
                            (gameObjects[xx, yy].transform as RectTransform).anchoredPosition =
                            Vector2.Lerp((gameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                                new Vector2(xx, -1), speed);
                    }
                    else if (yy > lines[0])
                    {
                        count = 0;
                        for (int i = 0; i < lines.Length; i++)
                            if (lines[i] < yy) count++;
                            else break;

                        for (int xx = 0; xx < controller.MaxX; xx++)
                            (gameObjects[xx, yy].transform as RectTransform).anchoredPosition =
                            Vector2.Lerp((gameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                                new Vector2(xx, yy - count), speed);
                    }
                }
            }

            for (int yy = 0; yy < controller.MaxY; yy++)
                for (int xx = 0; xx < controller.MaxX; xx++)
                    (gameObjects[xx, yy].transform as RectTransform).anchoredPosition = new Vector2(xx, yy);
        }
    }
}