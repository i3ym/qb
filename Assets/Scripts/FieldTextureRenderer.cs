using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FieldTextureRenderer : FieldRendererBase
{
    [SerializeField]
    RawImage NextObjectRawImage = null;

    RawImage FieldRawImage;
    Texture2D FieldTexture, NextObjectTexture;
    Color CurrentColor;
    Color black = Color.black;

    public override void Initialize()
    {
        base.Initialize();

        NextObjectTexture = new Texture2D(4, 4, TextureFormat.RGB24, 0, true);
        NextObjectTexture.filterMode = FilterMode.Point;
        NextObjectTexture.SetPixels(Enumerable.Repeat(black, 4 * 4).ToArray());
        NextObjectTexture.Apply();

        NextObjectRawImage.texture = NextObjectTexture;
        NextObjectRawImage.color = Color.white;
    }


    public override void SetMaxs(int maxx, int maxy)
    {
        FieldRawImage = GetComponent<RawImage>();
        if (FieldRawImage == null) FieldRawImage = gameObject.AddComponent<RawImage>();

        MaxX = maxx;
        MaxY = maxy;

        FieldTexture = new Texture2D(maxx, maxy, TextureFormat.RGB24, 0, true);
        FieldTexture.filterMode = FilterMode.Point;
        FieldTexture.SetPixels(Enumerable.Repeat(black, MaxX * MaxY).ToArray());
        FieldTexture.Apply();

        FieldRawImage.texture = FieldTexture;
        FieldRawImage.color = Color.white;
    }

    public override float RemoveLinesAnimation(List<int> y, GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        for (int i = y.Count - 1; i > -1; i--)
            for (int y2 = y[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    FieldTexture.SetPixel(x2, y2, FieldTexture.GetPixel(x2, y2 + 1));

        FieldTexture.Apply();
        FieldRawImage.texture = FieldTexture;

        return .2f;
    }

    public override void ShowNextShape(Shapes shape)
    {
        var obj = FieldController.ShapeObjects[shape];

        int addx = (4 - obj.GetLength(0)) / 2;
        int addy = (4 - obj.GetLength(1)) / 2;

        if (shape == Shapes.Z || shape == Shapes.ZR) addy++;

        NextObjectTexture.SetPixels(Enumerable.Repeat(black, 4 * 4).ToArray());

        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
            {
                if (x >= obj.GetLength(0) || y >= obj.GetLength(1)) continue;

                if (obj[x, y]) NextObjectTexture.SetPixel(x + addx, y + addy, ShapeColors[shape]);
            }

        NextObjectTexture.Apply();
        NextObjectRawImage.texture = NextObjectTexture;
    }
    public override void UpdateColors(GameMatrix gameMatrix, Shapes currentShape) => CurrentColor = ShapeColors[currentShape];
    public override void UpdateMatrix(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        if (Game.BlindRendering)
        {
            for (int x = 0; x < MaxX; x++)
                for (int y = MaxY - 1; y >= 0; y--)
                {
                    FieldTexture.SetPixel(x, y, currentObject[x - currentX, y - currentY] ? CurrentColor : black);

                    if (gameMatrix[x, y])
                    {
                        FieldTexture.SetPixel(x, y, CurrentColor);

                        for (int yy = y - 1; yy >= 0; yy--)
                            FieldTexture.SetPixel(x, yy, currentObject[x - currentX, yy - currentY] ? CurrentColor : black);

                        break;
                    }
                }
        }
        else
            for (int y = 0; y < MaxY; y++)
                for (int x = 0; x < MaxX; x++)
                {
                    if (currentObject[x - currentX, y - currentY])
                        FieldTexture.SetPixel(x, y, CurrentColor);
                    else if (!gameMatrix[x, y])
                        FieldTexture.SetPixel(x, y, black);
                }

        FieldTexture.Apply();
        FieldRawImage.texture = FieldTexture;
    }
}