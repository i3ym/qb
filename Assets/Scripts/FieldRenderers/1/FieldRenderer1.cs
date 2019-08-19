using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldRenderer1 : FieldCanvasRendererBase
{
    Color StartCameraColor;

    void Start() => StartCameraColor = Game.Camera.backgroundColor;

    public override float RemoveLinesAnimation(List<int> y, GameMatrix gameMatrix, bool[, ] currentObject, int X, int Y)
    {
        StartCoroutine(RemoveLinesAnimationCoroutine(y, gameMatrix, currentObject, X, Y));
        return 1f;
    }

    IEnumerator RemoveLinesAnimationCoroutine(List<int> y, GameMatrix gameMatrix, bool[, ] currentObject, int X, int Y)
    {
        float speed = 0f;

        Vector3 temp;
        float time = Time.time + 1f;
        int count;

        var wff = new WaitForFixedUpdate();

        while (time > Time.time)
        {
            temp = new Vector3(0, speed -= .01f);

            for (int yy = 0; yy < 20; yy++)
            {
                if (y.Contains(yy))
                {
                    for (int xx = 0; xx < 10; xx++)
                        (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((GameObjects[xx, yy].transform as RectTransform).anchoredPosition, new Vector2(xx + .5f - 5f, -1 + .5f - 10f), -speed);
                }
                else if (yy > y[0])
                {
                    count = 0;
                    for (int i = 0; i < y.Count; i++)
                        if (y[i] < yy) count++;
                        else break;

                    for (int xx = 0; xx < 10; xx++)
                        (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((GameObjects[xx, yy].transform as RectTransform).anchoredPosition, new Vector2(xx + .5f - 5f, yy - count + .5f - 10f), -speed);
                }
            }

            yield return wff;
        }

        Game.Camera.backgroundColor += Color.white * (y.Count / 4f - .1f);
        StartCoroutine(RevertCameraBackgroundColorCorotuine());

        /* var burst = TetrisEffectParticle.emission.GetBurst(0);
        burst.count = 10 * y.Count;
        TetrisEffectParticle.emission.SetBurst(0, burst);
        TetrisEffectParticle.Play();*/

        for (int yy = 0; yy < 20; yy++)
            for (int xx = 0; xx < 10; xx++)
                (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = new Vector2(xx + .5f - 5f, yy + .5f - 10f); // TODO check if this is working in canvas

        base.RemoveLinesAnimation(y, gameMatrix, currentObject, X, Y);

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