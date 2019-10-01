using System.Collections.Generic;
using System.Linq;
using qb.Shared;
using qb.Shared.Animations;
using UnityEngine;

public class RemoveLinesAnimationSlideDown : RemoveLinesAnimation<UnityField>
{
    protected override IEnumerator<bool> Animation(int[] lines, IFieldControllerInfo controller, UnityField field)
    {
        float speed = .1f;
        float time = Time.time + 1f;
        int count;

        while (time > Time.time)
        {
            speed += .001f;
            yield return true;

            for (int yy = 0; yy < controller.MaxY; yy++)
            {
                if (lines.Any(x => x == yy))
                {
                    for (int xx = 0; xx < controller.MaxX; xx++)
                        (field.GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((field.GameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                            new Vector2(xx, -1), speed);
                }
                else if (yy > lines[0])
                {
                    count = 0;
                    for (int i = 0; i < lines.Length; i++)
                        if (lines[i] < yy) count++;
                        else break;

                    for (int xx = 0; xx < controller.MaxX; xx++)
                        (field.GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((field.GameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                            new Vector2(xx, yy - count), speed);
                }
            }
        }

        for (int yy = 0; yy < controller.MaxY; yy++)
            for (int xx = 0; xx < controller.MaxX; xx++)
                (field.GameObjects[xx, yy].transform as RectTransform).anchoredPosition = new Vector2(xx, yy);
    }
}