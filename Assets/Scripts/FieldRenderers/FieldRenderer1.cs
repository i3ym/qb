using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldRenderer1 : FieldObjectRenderer
{
    Color StartCameraColor;

    void Start() => StartCameraColor = Game.Camera.backgroundColor;

    protected override IEnumerator<bool> _RemoveLinesAnimation(int[] lines)
    {
        float speed = .1f;
        float time = Time.time + 1f;
        int count;

        var wff = new WaitForFixedUpdate();

        while (time > Time.time)
        {
            speed += .001f;
            yield return true;

            for (int yy = 0; yy < MaxY; yy++)
            {
                if (lines.Any(x => x == yy))
                {
                    for (int xx = 0; xx < MaxX; xx++)
                        (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((GameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                            new Vector2(xx, -1), speed);
                }
                else if (yy > lines[0])
                {
                    count = 0;
                    for (int i = 0; i < lines.Length; i++)
                        if (lines[i] < yy) count++;
                        else break;

                    for (int xx = 0; xx < MaxX; xx++)
                        (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = Vector2.Lerp((GameObjects[xx, yy].transform as RectTransform).anchoredPosition,
                            new Vector2(xx, yy - count), speed);
                }
            }
        }

        for (int yy = 0; yy < MaxY; yy++)
            for (int xx = 0; xx < MaxX; xx++)
                (GameObjects[xx, yy].transform as RectTransform).anchoredPosition = new Vector2(xx, yy);

        Game.Camera.backgroundColor += Color.white * (lines.Length / 4f - .1f);
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