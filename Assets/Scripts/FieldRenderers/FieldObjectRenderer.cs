using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class FieldObjectRenderer : FieldRendererBase
{
    public static int Skin
    {
        get => Config.SelectedSkin;
        set
        {
            Config.SelectedSkin = value;
            RecreateUvs();
        }
    }

    [SerializeField]
    protected GameObject FieldGameObjectPrefab = null;
    [SerializeField]
    protected Transform NextObjectsParent = null;

    protected RawImage[, ] GameObjects, NextObjects;

    protected static Dictionary<Shapes, Rect> Uvs = new Dictionary<Shapes, Rect>();

    protected static Texture2D Atlas => Config.Instance.Atlas;

    public override void Initialize(IFieldController controller)
    {
        base.Initialize(controller);

        RecreateUvs();
    }

    static void RecreateUvs()
    {
        Uvs.Clear();

        var shapes = new Shapes[] { Shapes.G, Shapes.ZR, Shapes.GR, Shapes.O, Shapes.T, Shapes.Z, Shapes.I };
        for (int i = 0; i < shapes.Length; i++)
            Uvs.Add(shapes[i], new Rect(16f / Atlas.width * i, 1f - (16f / Atlas.height * Skin), 16f / Atlas.width, 16f / Atlas.height));
    }

    public override void CreateField()
    {
        if (GameObjects != null)
            foreach (var img in GameObjects)
                Destroy(img.gameObject);

        GameObjects = new RawImage[MaxX, MaxY];

        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                GameObjects[x, y] = GetNewCube(new Vector2(x - MaxX / 2f, y - MaxY / 2f)).GetComponent<RawImage>();

        if (NextObjects != null)
            foreach (var img in NextObjects)
                Destroy(img.gameObject);

        NextObjects = new RawImage[4, 4];

        GameObject cube;
        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
            {
                cube = GetNewCube(new Vector2(x - MaxX / 2f, y - MaxY / 2f));
                cube.transform.SetParent(NextObjectsParent, false);
                (cube.transform as RectTransform).anchoredPosition = new Vector2(x, y);

                NextObjects[x, y] = cube.GetComponent<RawImage>();
            }
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
    /* protected virtual GameObject CreateNextObject(Shapes shape)
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
     }*/

    protected override void _UpdateMatrix()
    {
        if (Game.BlindRendering)
        {
            for (int x = 0; x < MaxX; x++)
                for (int y = MaxY - 1; y >= 0; y--)
                {
                    GameObjects[x, y].gameObject.SetActive(CurrentObject[x - CurrentX, y - CurrentY]);

                    if (GameMatrix[x, y])
                    {
                        GameObjects[x, y].gameObject.SetActive(true);

                        for (int yy = y - 1; yy >= 0; yy--)
                            GameObjects[x, yy].gameObject.SetActive(CurrentObject[x - CurrentX, yy - CurrentY]);

                        break;
                    }
                }
        }
        else
            for (int y = 0; y < MaxY; y++)
                for (int x = 0; x < MaxX; x++)
                    GameObjects[x, y].gameObject.SetActive(GameMatrix[x, y] || CurrentObject[x - CurrentX, y - CurrentY]);

        UpdateColors();
    }
    protected override void _UpdateMatrixFast()
    {
        _UpdateMatrix();
        return;

        if (Game.BlindRendering)
        {
            for (int x = 0; x < MaxX; x++)
                for (int y = MaxY - 1; y >= 0; y--)
                {
                    GameObjects[x, y].gameObject.SetActive(CurrentObject[x - CurrentX, y - CurrentY]);

                    if (GameMatrix[x, y])
                    {
                        GameObjects[x, y].gameObject.SetActive(true);

                        for (int yy = y - 1; yy >= 0; yy--)
                            GameObjects[x, yy].gameObject.SetActive(CurrentObject[x - CurrentX, yy - CurrentY]);

                        break;
                    }
                }
        }
        else
            for (int y = 0; y < MaxY; y++)
                for (int x = 0; x < MaxX; x++)
                {
                    if (!GameMatrix.IsInBounds(x, y)) continue;

                    if (CurrentObject[x - CurrentX, y - CurrentY])
                        GameObjects[x, y].gameObject.SetActive(true);
                    else if (!GameMatrix[x, y])
                        GameObjects[x, y].gameObject.SetActive(false);
                }

        UpdateColors();
    }
    public override void UpdateColors()
    {
        for (int y = 0; y < CurrentObject.MaxX; y++)
            for (int x = 0; x < CurrentObject.MaxY; x++)
                if (CurrentObject[x, y] && CurrentX + x >= 0 && CurrentY + y >= 0 && CurrentX + x < GameObjects.GetLength(0) && CurrentY + y < GameObjects.GetLength(1))
                    SetMaterial(GameObjects[CurrentX + x, CurrentY + y], CurrentShape);
    }
    public override void ShowNextShape()
    {
        var nextMatrix = FieldController.ShapeObjects[NextShape];

        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
            {
                NextObjects[x, y].gameObject.SetActive(nextMatrix[x, y]);
                SetMaterial(NextObjects[x, y], NextShape);
            }
    }

    public override float RemoveLinesAnimation(List<int> listy, GameMatrix GameMatrix, GameMatrix CurrentObject, int x, int y)
    {
        for (int i = listy.Count - 1; i > -1; i--)
            for (int y2 = listy[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    SetMaterial(GameObjects[x2, y2], GameObjects[x2, y2 + 1]);

        return .1f;
    }
}