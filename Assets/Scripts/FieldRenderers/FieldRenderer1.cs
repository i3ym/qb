using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldRenderer1 : FieldObjectRendererBase
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

        Vector3 temp;
        float time = Time.time + 1f;
        int count;

        var wff = new WaitForFixedUpdate();

        while (time > Time.time)
        {
            temp = new Vector3(0, speed -= .01f);

            for (int yy = 0; yy < MaxY; yy++)
            {
                if (listy.Contains(yy))
                {
                    for (int xx = 0; xx < MaxX; xx++)
                        (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((GameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                            new Vector2(xx + .5f - MaxX / 2f, -1 + .5f - MaxY / 2f), -speed);
                }
                else if (yy > listy[0])
                {
                    count = 0;
                    for (int i = 0; i < listy.Count; i++)
                        if (listy[i] < yy) count++;
                        else break;

                    for (int xx = 0; xx < MaxX; xx++)
                        (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((GameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                            new Vector2(xx + .5f - MaxX / 2f, yy - count + .5f - MaxY / 2f), -speed);
                }
            }

            yield return wff;
        }

        Game.Camera.backgroundColor += Color.white * (listy.Count / 4f - .1f);
        StartCoroutine(RevertCameraBackgroundColorCorotuine());

        /* var burst = TetrisEffectParticle.emission.GetBurst(0);
        burst.count = 10 * y.Count;
        TetrisEffectParticle.emission.SetBurst(0, burst);
        TetrisEffectParticle.Play();*/

        for (int yy = 0; yy < MaxY; yy++)
            for (int xx = 0; xx < MaxX; xx++)
                (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = new Vector2(xx + .5f - MaxX / 2f, yy + .5f - MaxY / 2f);

        base.RemoveLinesAnimation(listy, gameMatrix, currentObject, x, y);

        // if (updateBgColor) SetHeightColor(gameMatrix);
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