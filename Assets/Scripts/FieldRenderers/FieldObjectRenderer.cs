using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FieldObjectRendererBase : FieldRendererBase
{
    [SerializeField]
    protected GameObject FieldGameObjectPrefab = null;
    [SerializeField]
    protected Transform NextObjectsParent = null;

    protected RawImage[, ] GameObjects;
    protected GameObject[] NextObjects = new GameObject[7];

    protected Dictionary<Shapes, Rect> Uvs = new Dictionary<Shapes, Rect>();

    protected Texture2D Atlas => Config.Instance.Atlas;

    public override void Initialize()
    {
        base.Initialize();

        var shapes = new Shapes[] { Shapes.G, Shapes.ZR, Shapes.GR, Shapes.O, Shapes.T, Shapes.Z, Shapes.I };
        for (int i = 0; i < shapes.Length; i++)
            Uvs.Add(shapes[i], new Rect(16f / Atlas.width * i, 1f - (16f / Atlas.height), 16f / Atlas.width, 16f / Atlas.height));

        for (int i = 0; i < 7; i++) NextObjects[i] = CreateNextObject((Shapes) i);
    }

    public override void SetMaxs(int maxx, int maxy)
    {
        MaxX = maxx;
        MaxY = maxy;

        GameObjects = new RawImage[maxx, maxy];

        for (int y = 0; y < maxy; y++)
            for (int x = 0; x < maxx; x++)
                GameObjects[x, y] = GetNewCube(new Vector2(x - MaxX / 2f, y - MaxY / 2f)).GetComponent<RawImage>();
    }

    protected void SetMaterial(RawImage img, Shapes shape) => img.uvRect = Uvs[shape];
    protected void SetMaterial(RawImage img, RawImage img2) => img.uvRect = img2.uvRect;

    protected virtual GameObject GetNewCube(Vector3 pos)
    {
        GameObject go = Instantiate(FieldGameObjectPrefab, pos, Quaternion.identity, transform);
        (go.transform as RectTransform).anchoredPosition = pos + new Vector3(.5f, .5f);
        go.SetActive(false);

        go.GetComponent<RawImage>().texture = Atlas;

        return go;
    }
    protected virtual GameObject GetNewNextCube(Vector3 pos, Transform parent, Shapes shape)
    {
        var cube = GetNewCube(pos);
        cube.transform.SetParent(parent, false);
        (cube.transform as RectTransform).anchoredPosition = pos;

        SetMaterial(cube.GetComponent<RawImage>(), shape);
        return cube;
    }
    protected virtual GameObject CreateNextObject(Shapes shape)
    {
        var go = new GameObject("NEXT_" + shape.ToString());
        go.transform.SetParent(NextObjectsParent);
        go.transform.localPosition = new Vector3(.5f, .5f);
        go.transform.localScale = Vector3.one;

        int addx = 0, addy = 0;

        if (shape == Shapes.I || shape == Shapes.GR || shape == Shapes.G || shape == Shapes.T) addy = -1;
        if (shape == Shapes.O) addx = 1;

        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
            {
                try
                {
                    if (Game.ShapeObjects[shape][x, y]) GetNewNextCube(new Vector3(x + addx, y + addy), go.transform, shape).SetActive(true);
                }
                catch { }
            }

        go.SetActive(false);
        return go;
    }

    protected override void _UpdateMatrix(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        if (Game.BlindRendering)
        {
            for (int x = 0; x < MaxX; x++)
                for (int y = MaxY - 1; y >= 0; y--)
                {
                    GameObjects[x, y].gameObject.SetActive(currentObject[x - currentX, y - currentY]);

                    if (gameMatrix[x, y])
                    {
                        GameObjects[x, y].gameObject.SetActive(true);

                        for (int yy = y - 1; yy >= 0; yy--)
                            GameObjects[x, yy].gameObject.SetActive(currentObject[x - currentX, yy - currentY]);

                        break;
                    }
                }
        }
        else
            for (int y = 0; y < MaxY; y++)
                for (int x = 0; x < MaxX; x++)
                    GameObjects[x, y].gameObject.SetActive(gameMatrix[x, y] || currentObject[x - currentX, y - currentY]);
    }
    protected override void _UpdateMatrixFast(GameMatrix gameMatrix, GameMatrix currentObject, int currentX, int currentY)
    {
        _UpdateMatrix(gameMatrix, currentObject, currentX, currentY);
        return;

        if (Game.BlindRendering)
        {
            for (int x = 0; x < MaxX; x++)
                for (int y = MaxY - 1; y >= 0; y--)
                {
                    GameObjects[x, y].gameObject.SetActive(currentObject[x - currentX, y - currentY]);

                    if (gameMatrix[x, y])
                    {
                        GameObjects[x, y].gameObject.SetActive(true);

                        for (int yy = y - 1; yy >= 0; yy--)
                            GameObjects[x, yy].gameObject.SetActive(currentObject[x - currentX, yy - currentY]);

                        break;
                    }
                }
        }
        else
            for (int y = 0; y < MaxY; y++)
                for (int x = 0; x < MaxX; x++)
                {
                    if (!gameMatrix.IsInBounds(x, y)) continue;

                    if (currentObject[x - currentX, y - currentY])
                        GameObjects[x, y].gameObject.SetActive(true);
                    else if (!gameMatrix[x, y])
                        GameObjects[x, y].gameObject.SetActive(false);
                }
    }
    public override void UpdateColors(GameMatrix gameMatrix, Shapes currentShape)
    {
        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                if (!gameMatrix[x, y]) SetMaterial(GameObjects[x, y], currentShape);
    }
    public override void ShowNextShape(Shapes shape)
    {
        for (int i = 0; i < 7; i++) NextObjects[i].SetActive(false);
        NextObjects[(int) shape].SetActive(true);
    }

    public override float RemoveLinesAnimation(List<int> listy, GameMatrix gameMatrix, GameMatrix currentObject, int x, int y)
    {
        for (int i = listy.Count - 1; i > -1; i--)
            for (int y2 = listy[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    SetMaterial(GameObjects[x2, y2], GameObjects[x2, y2 + 1]);

        return .1f;
    }
}