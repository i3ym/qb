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

    protected static Dictionary<Shapes, Color> ShapeColors = new Dictionary<Shapes, Color>();

    static FieldTextureRenderer()
    {
        ShapeColors.Clear();
        ShapeColors.Add(Shapes.G, Color.blue);
        ShapeColors.Add(Shapes.GR, new Color(1f, .5f, 0f));
        ShapeColors.Add(Shapes.Z, Color.red);
        ShapeColors.Add(Shapes.ZR, Color.green);
        ShapeColors.Add(Shapes.I, new Color(0f, 1f, 1f));
        ShapeColors.Add(Shapes.T, new Color(.5f, 0f, 1f));
        ShapeColors.Add(Shapes.O, Color.yellow);
    }

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

    [MethodImpl(256)]
    void SetState(int x, int y, Color color) => CurrentState[x + (y * MaxX)] = color;

    [MethodImpl(256)]
    Color GetState(int x, int y) => CurrentState[x + (y * MaxX)];

    public override void CreateField()
    {
        CurrentState = Enumerable.Repeat(black, MaxX * MaxY).ToArray();

        FieldTexture = new Texture2D(MaxX, MaxY, TextureFormat.RGB24, 0, true);
        FieldTexture.filterMode = FilterMode.Point;
        FieldTexture.SetPixels(CurrentState);
        FieldTexture.Apply();

        FieldRawImage.texture = FieldTexture;
        FieldRawImage.color = Color.white;
    }

    public override IEnumerator<bool> RemoveLinesAnimation(int[] lines)
    {
        for (int i = lines.Length - 1; i > -1; i--)
            for (int y = lines[i]; y < MaxY - 1; y++)
                for (int x = 0; x < MaxX; x++)
                    SetState(x, y, GetState(x, y + 1));

        FieldTexture.SetPixels(CurrentState);
        FieldTexture.Apply();
        FieldRawImage.texture = FieldTexture;

        for (byte i = 0; i < 30; i++) yield return true;
    }

    public override void ShowNextShape()
    {
        var obj = FieldController.ShapeObjects[NextShape];

        int addx = (4 - obj.MaxX) / 2;
        int addy = (4 - obj.MaxY) / 2;

        if (NextShape == Shapes.Z || NextShape == Shapes.ZR) addy++;

        NextObjectTexture.SetPixels(Enumerable.Repeat(black, 4 * 4).ToArray());

        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
            {
                if (x >= obj.MaxX || y >= obj.MaxY) continue;

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

        FieldTexture.SetPixels(CurrentState);
        FieldTexture.Apply();
        FieldRawImage.texture = FieldTexture;
    }

    public override IEnumerator<bool> GameOverAnimation()
    {
        yield return true;
    }
}