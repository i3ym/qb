using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FieldObjectRenderer : FieldRendererBase
{
    [SerializeField]
    protected GameObject FieldGameObjectPrefab = null;
    [SerializeField]
    protected Transform NextObjectsParent = null;

    protected GameMatrix<RawImage> GameObjects, NextObjects;

    protected static Dictionary<Shapes, Rect> Uvs = new Dictionary<Shapes, Rect>();
    protected GameMatrix<BackgroundMaker.Tile> Tiles;

    protected static Texture2D Atlas => Config.Instance.Atlas;

    public override void Initialize(IFieldController controller)
    {
        base.Initialize(controller);

        Tiles = new GameMatrix<BackgroundMaker.Tile>(MaxX, MaxY);

        Config.OnSkinChange += () => RecreateUvs();
        Config.SelectedSkin = Config.SelectedSkin;
    }

    static void RecreateUvs()
    {
        Uvs.Clear();

        var shapes = new Shapes[] { Shapes.G, Shapes.ZR, Shapes.GR, Shapes.O, Shapes.T, Shapes.Z, Shapes.I };
        for (int i = 0; i < shapes.Length; i++)
            Uvs.Add(shapes[i], new Rect(16f / Atlas.width * i, 1f - (16f / Atlas.height * (Config.SelectedSkin + 1)), 16f / Atlas.width, 16f / Atlas.height));
    }

    public override void CreateField()
    {
        if (GameObjects != null)
            foreach (var img in GameObjects)
                Destroy(img.gameObject);

        GameObjects = new RawImage[MaxX, MaxY];

        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                GameObjects[x, y] = GetNewCube(new Vector2(x, y)).GetComponent<RawImage>();

        if (NextObjects != null)
            foreach (var img in NextObjects)
                Destroy(img.gameObject);

        NextObjects = new RawImage[4, 4];

        GameObject cube;
        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
            {
                cube = GetNewCube(new Vector2(x, y));
                cube.transform.SetParent(NextObjectsParent, false);
                (cube.transform as RectTransform).anchoredPosition = new Vector2(x, y);

                NextObjects[x, y] = cube.GetComponent<RawImage>();
            }
    }

    protected void SetMaterial(RawImage img, Shapes shape)
    {
        if (img != null)
            img.uvRect = Uvs[shape];
    }
    protected void SetMaterial(RawImage img, RawImage img2)
    {
        if (img != null && img2 != null)
            img.uvRect = img2.uvRect;
    }

    protected virtual GameObject GetNewCube(Vector3 pos)
    {
        GameObject go = Instantiate(FieldGameObjectPrefab, pos, Quaternion.identity, transform);
        (go.transform as RectTransform).pivot = (go.transform as RectTransform).anchorMax = (go.transform as RectTransform).anchorMin = Vector2.zero;
        (go.transform as RectTransform).anchoredPosition = pos;
        go.SetActive(false);

        go.GetComponent<RawImage>().texture = Atlas;

        return go;
    }

    protected override void _UpdateMatrix()
    {
        for (int y = 0; y < CurrentObject.MaxY; y++)
            for (int x = 0; x < CurrentObject.MaxX; x++)
                if (CurrentObject[x, y])
                    Tiles[CurrentX + x, CurrentY + y] = ShapeConverter.ToTile(CurrentShape);

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
        for (int y = 0; y < CurrentObject.MaxY; y++)
            for (int x = 0; x < CurrentObject.MaxX; x++)
                if (CurrentObject[x, y])
                    Tiles[CurrentX + x, CurrentY + y] = ShapeConverter.ToTile(CurrentShape);

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
        for (int y = 0; y < GameMatrix.MaxY; y++)
            for (int x = 0; x < GameMatrix.MaxX; x++)
                SetMaterial(GameObjects[x, y], ShapeConverter.ToShape(Tiles[x, y]));
    }
    public override void ShowNextShape()
    {
        var nextMatrix = FieldController.ShapeObjects[NextShape];

        int addx = 0;
        int addy = 0;

        if (NextShape == Shapes.O) addx = 1;
        else if (NextShape != Shapes.I) addy = -1;

        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
                NextObjects[x, y].gameObject.SetActive(false);

        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
            {
                if (NextObjects[x + addx, y + addy] == null) continue;

                NextObjects[x + addx, y + addy].gameObject.SetActive(nextMatrix[x, y]);
                SetMaterial(NextObjects[x + addx, y + addy], NextShape);
            }
    }

    public override IEnumerator<bool> RemoveLinesAnimation(int[] lines)
    {
        var animation = _RemoveLinesAnimation(lines);

        while (animation.MoveNext() && animation.Current)
            yield return true;

        for (int i = lines.Length - 1; i > -1; i--)
            for (int y2 = lines[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    SetMaterial(GameObjects[x2, y2], GameObjects[x2, y2 + 1]);
    }
    protected abstract IEnumerator<bool> _RemoveLinesAnimation(int[] lines);
}