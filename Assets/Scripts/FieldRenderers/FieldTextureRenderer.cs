using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    Color[] CurrentState;

    public override void Initialize()
    {
        base.Initialize();

        FieldRawImage = GetComponent<RawImage>();
        if (FieldRawImage == null) FieldRawImage = gameObject.AddComponent<RawImage>();

        NextObjectTexture = new Texture2D(4, 4, TextureFormat.RGB24, 0, true);
        NextObjectTexture.filterMode = FilterMode.Point;
        NextObjectTexture.SetPixels(Enumerable.Repeat(black, 4 * 4).ToArray());
        NextObjectTexture.Apply();

        NextObjectRawImage.texture = NextObjectTexture;
        NextObjectRawImage.color = Color.white;
    }

    //

    [MethodImpl(256)]
    void SetState(int x, int y, Color color) => CurrentState[x + (y * MaxX)] = color;

    [MethodImpl(256)]
    Color GetState(int x, int y) => CurrentState[x + (y * MaxX)];

    public override void SetMaxs(int maxx, int maxy)
    {
        MaxX = maxx;
        MaxY = maxy;

        CurrentState = Enumerable.Repeat(black, MaxX * MaxY).ToArray();

        FieldTexture = new Texture2D(maxx, maxy, TextureFormat.RGB24, 0, true);
        FieldTexture.filterMode = FilterMode.Point;
        FieldTexture.SetPixels(CurrentState);
        FieldTexture.Apply();

        FieldRawImage.texture = FieldTexture;
        FieldRawImage.color = Color.white;
    }

    public override float RemoveLinesAnimation(List<int> y, GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        for (int i = y.Count - 1; i > -1; i--)
            for (int y2 = y[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    SetState(x2, y2, GetState(x2, y2 + 1));

        FieldTexture.SetPixels(CurrentState);
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
    protected override void _UpdateMatrix(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        if (Game.BlindRendering)
        {
            for (int x = 0; x < MaxX; x++)
                for (int y = MaxY - 1; y >= 0; y--)
                {
                    if (gameMatrix[x, y])
                    {
                        SetState(x, y, CurrentColor);

                        for (int yy = y - 1; yy >= 0; yy--)
                            SetState(x, yy, currentObject[x - currentX, yy - currentY] ? CurrentColor : black);

                        break;
                    }
                    else SetState(x, y, currentObject[x - currentX, y - currentY] ? CurrentColor : black);
                }
        }
        else
        {
            for (int y = 0; y < MaxY; y++)
                for (int x = 0; x < MaxX; x++)
                {
                    if (currentObject[x - currentX, y - currentY])
                        SetState(x, y, CurrentColor);
                    else if (!gameMatrix[x, y])
                        SetState(x, y, black);
                }
        }

        FieldTexture.SetPixels(CurrentState);
        FieldTexture.Apply();
        FieldRawImage.texture = FieldTexture;
    }
    protected override void _UpdateMatrixFast(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        _UpdateMatrix(gameMatrix, currentObject, currentX, currentY);

        return;

        if (Game.BlindRendering) _UpdateMatrix(gameMatrix, currentObject, currentX, currentY);
        else
            for (int y = currentY; y < currentY + currentObject.MaxY + 1; y++)
                for (int x = currentX - 1; x < currentX + currentObject.MaxX + 1; x++)
                {
                    if (x < 0 || y < 0 || x >= MaxX || y >= MaxY) continue; // gameMatrix.IsInBounds(x, y); copied for performance resons

                    if (currentObject[x - currentX, y - currentY])
                        CurrentState[x + (y * MaxX)] = CurrentColor; // SetState(x, y, CurrentColor); copied for performance reasons
                    else if (!gameMatrix.GetWithoutChecks(x, y))
                        CurrentState[x + (y * MaxX)] = black; // SetState(x, y, black); copied for performance reasons
                }

        // TODO fix pixels when moving more that 1 pixel in one frame

        FieldTexture.SetPixels(CurrentState);
        FieldTexture.Apply();
        FieldRawImage.texture = FieldTexture;
    }
}