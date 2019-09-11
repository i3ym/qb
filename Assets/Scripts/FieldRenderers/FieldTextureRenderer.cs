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

    public override void Initialize(IFieldController controller)
    {
        base.Initialize(controller);

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

    public override float RemoveLinesAnimation(List<int> y, GameMatrix gameMatrix, GameMatrix CurrentObject, int CurrentX, int CurrentY)
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

    public override void ShowNextShape()
    {
        var obj = FieldController.ShapeObjects[NextShape];

        int addx = (4 - obj.GetLength(0)) / 2;
        int addy = (4 - obj.GetLength(1)) / 2;

        if (NextShape == Shapes.Z || NextShape == Shapes.ZR) addy++;

        NextObjectTexture.SetPixels(Enumerable.Repeat(black, 4 * 4).ToArray());

        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
            {
                if (x >= obj.GetLength(0) || y >= obj.GetLength(1)) continue;

                if (obj[x, y]) NextObjectTexture.SetPixel(x + addx, y + addy, ShapeColors[NextShape]);
            }

        NextObjectTexture.Apply();
        NextObjectRawImage.texture = NextObjectTexture;
    }
    public override void UpdateColors() => CurrentColor = ShapeColors[CurrentShape];
    protected override void _UpdateMatrix()
    {
        if (Game.BlindRendering)
        {
            for (int x = 0; x < MaxX; x++)
                for (int y = MaxY - 1; y >= 0; y--)
                {
                    if (GameMatrix[x, y])
                    {
                        SetState(x, y, CurrentColor);

                        for (int yy = y - 1; yy >= 0; yy--)
                            SetState(x, yy, CurrentObject[x - CurrentX, yy - CurrentY] ? CurrentColor : black);

                        break;
                    }
                    else SetState(x, y, CurrentObject[x - CurrentX, y - CurrentY] ? CurrentColor : black);
                }
        }
        else
        {
            for (int y = 0; y < MaxY; y++)
                for (int x = 0; x < MaxX; x++)
                {
                    if (CurrentObject[x - CurrentX, y - CurrentY])
                        SetState(x, y, CurrentColor);
                    else if (!GameMatrix[x, y])
                        SetState(x, y, black);
                }
        }

        FieldTexture.SetPixels(CurrentState);
        FieldTexture.Apply();
        FieldRawImage.texture = FieldTexture;
    }
    protected override void _UpdateMatrixFast()
    {
        _UpdateMatrix();

        return;

        if (Game.BlindRendering) _UpdateMatrix();
        else
            for (int y = CurrentY; y < CurrentY + CurrentObject.MaxY + 1; y++)
                for (int x = CurrentX - 1; x < CurrentX + CurrentObject.MaxX + 1; x++)
                {
                    if (x < 0 || y < 0 || x >= MaxX || y >= MaxY) continue; // gameMatrix.IsInBounds(x, y); copied for performance resons

                    if (CurrentObject[x - CurrentX, y - CurrentY])
                        CurrentState[x + (y * MaxX)] = CurrentColor; // SetState(x, y, CurrentColor); copied for performance reasons
                    else if (!GameMatrix.GetWithoutChecks(x, y))
                        CurrentState[x + (y * MaxX)] = black; // SetState(x, y, black); copied for performance reasons
                }

        // TODO fix pixels when moving more that 1 pixel in one frame

        FieldTexture.SetPixels(CurrentState);
        FieldTexture.Apply();
        FieldRawImage.texture = FieldTexture;
    }
}