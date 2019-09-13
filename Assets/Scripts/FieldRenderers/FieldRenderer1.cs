using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldRenderer1 : FieldObjectRenderer
{
    Color StartCameraColor;

    void Start() => StartCameraColor = Game.Camera.backgroundColor;

    public override float RemoveLinesAnimation(List<int> listy, GameMatrix gameMatrix, GameMatrix currentObject, int x, int y)
    {
        StartCoroutine(RemoveLinesAnimationCoroutine(listy, gameMatrix, currentObject, x, y));
        return 1f;
    }

    IEnumerator RemoveLinesAnimationCoroutine(List<int> listy, GameMatrix gameMatrix, GameMatrix currentObject, int x, int y)
    {
        float speed = 0f;

        float time = Time.time + 1f;
        int count;

        var wff = new WaitForFixedUpdate();

        while (time > Time.time)
        {
            yield return wff;

            speed -= .01f;

            for (int yy = 0; yy < MaxY; yy++)
            {
                if (listy.Contains(yy))
                {
                    for (int xx = 0; xx < MaxX; xx++)
                        (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((GameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                            new Vector2(xx, -1), -speed);
                }
                else if (yy > listy[0])
                {
                    count = 0;
                    for (int i = 0; i < listy.Count; i++)
                        if (listy[i] < yy) count++;
                        else break;

                    for (int xx = 0; xx < MaxX; xx++)
                        (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((GameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                            new Vector2(xx, yy - count), -speed);
                }
            }
        }

        for (int yy = 0; yy < MaxY; yy++)
            for (int xx = 0; xx < MaxX; xx++)
                (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = new Vector2(xx, yy);

        base.RemoveLinesAnimation(listy, gameMatrix, currentObject, x, y);

        Game.Camera.backgroundColor += Color.white * (listy.Count / 4f - .1f);
        StartCoroutine(RevertCameraBackgroundColorCorotuine());
    }

    IEnumerator RevertCameraBackgroundColorCorotuine()
    {
        var time = Time.time + .5f;
        var wff = new WaitForFixedUpdate();

        while (time > Time.time)
        {
            Game.Camera.backgroundColor = Color.Lerp(Game.Camera.backgroundColor, StartCameraColor, .1f);
            yield return wff;
        }
    }
}