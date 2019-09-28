using System.Collections.Generic;
using System.Linq;
using qb.Shared;
using UnityEngine;
using UnityEngine.UI;

public class FieldObjectRenderer : FieldRendererBase
{
    protected static Dictionary<Shape, Rect> Uvs = new Dictionary<Shape, Rect>();

    public readonly FieldRendererUnityInfo FieldRendererInfo;
    protected GameMatrix<CubeTile> Tiles;

    protected GameObject FieldGameObjectPrefab { get => FieldRendererInfo.FieldGameObjectPrefab; set => FieldRendererInfo.FieldGameObjectPrefab = value; }
    protected Transform NextObjectsParent { get => FieldRendererInfo.NextObjectsParent; set => FieldRendererInfo.NextObjectsParent = value; }
    protected GameMatrix<RawImage> GameObjects { get => FieldRendererInfo.GameObjects; set => FieldRendererInfo.GameObjects = value; }
    protected GameMatrix<RawImage> NextObjects { get => FieldRendererInfo.NextObjects; set => FieldRendererInfo.NextObjects = value; }

    protected static Texture2D Atlas => Config.Instance.Atlas;

    public FieldObjectRenderer(FieldRendererUnityInfo info)
    {
        FieldRendererInfo = info;

        Tiles = new GameMatrix<CubeTile>(MaxX, MaxY);

        Config.OnSkinChange += () => RecreateUvs();
        Config.SelectedSkin = Config.SelectedSkin;

        CreateField();
    }

    protected virtual void OnEnable() => NextObjectsParent.gameObject.SetActive(true);
    protected virtual void OnDisable() => NextObjectsParent.gameObject.SetActive(false);

    static void RecreateUvs()
    {
        Uvs.Clear();

        var shapes = new [] { Shapes.G, Shapes.ZR, Shapes.GR, Shapes.O, Shapes.T, Shapes.Z, Shapes.I };
        for (int i = 0; i < shapes.Length; i++)
            Uvs.Add(shapes[i], new Rect(16f / Atlas.width * i, 1f - (16f / Atlas.height * (Config.SelectedSkin + 1)), 16f / Atlas.width, 16f / Atlas.height));
    }

    void CreateField()
    {
        if (GameObjects != null)
            foreach (var img in GameObjects)
                GameObject.Destroy(img.gameObject);

        GameObjects = new RawImage[MaxX, MaxY];

        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                GameObjects[x, y] = GetNewCube(new Vector2(x, y)).GetComponent<RawImage>();

        if (NextObjects != null)
            foreach (var img in NextObjects)
                GameObject.Destroy(img.gameObject);

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

    protected void SetMaterial(RawImage img, Shape shape)
    {
        if (img != null)
            img.uvRect = Uvs[shape];
    }
    protected void SetMaterial(RawImage img, CubeTile tile)
    {
        if (img != null)
            img.uvRect = Uvs[Shapes.FromTile(tile)];
    }
    protected void SetMaterial(RawImage img, RawImage img2)
    {
        if (img != null && img2 != null)
            img.uvRect = img2.uvRect;
    }

    protected virtual GameObject GetNewCube(Vector3 pos)
    {
        GameObject go = GameObject.Instantiate(FieldGameObjectPrefab, pos, Quaternion.identity, FieldRendererInfo.transform);
        (go.transform as RectTransform).pivot = (go.transform as RectTransform).anchorMax = (go.transform as RectTransform).anchorMin = Vector2.zero;
        (go.transform as RectTransform).anchoredPosition = pos;
        go.SetActive(false);

        go.GetComponent<RawImage>().texture = Atlas;

        return go;
    }

    public override IEnumerator<bool> GameOverAnimation()
    {
        var uv = new Rect(16f / Atlas.width * (byte) CubeTile.Background,
            1f - (16f / Atlas.height * (Config.SelectedSkin + 1)), 16f / Atlas.width, 16f / Atlas.height);

        for (int y = GameMatrix.MaxY - 1; y >= GameMatrix.MaxY / 2; y--)
            for (int x = 0; x < GameMatrix.MaxX; x++)
            {
                GameObjects[x, y].gameObject.SetActive(true);
                GameObjects[x, y].uvRect = uv;

                GameObjects[GameMatrix.MaxX - 1 - x, GameMatrix.MaxY - 1 - y].gameObject.SetActive(true);
                GameObjects[GameMatrix.MaxX - 1 - x, GameMatrix.MaxY - 1 - y].uvRect = uv;

                yield return true;
            }
    }

    public override void UpdateLevelText()
    { // TODO
    }
    public override void UpdateScoreText()
    { // TODO
    }
    public override void UpdateLinesText()
    { // TODO
    }
    public override void UpdateTimeText()
    { // TODO
    }

    protected override void _UpdateMatrix()
    {
        for (int y = 0; y < CurrentObject.MaxY; y++)
            for (int x = 0; x < CurrentObject.MaxX; x++)
                if (CurrentObject[x, y])
                    Tiles[CurrentX + x, CurrentY + y] = CurrentShape.Tile;

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
                    Tiles[CurrentX + x, CurrentY + y] = CurrentShape.Tile;

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
                SetMaterial(GameObjects[x, y], Tiles[x, y]);
    }
    public override void ShowNextShape()
    {
        var nextMatrix = NextShape.Matrix;

        int addx = 0;
        int addy = 0;

        if (NextShape == Shapes.O) addx = 1;
        else addy = -1;

        for (int y = 0; y < NextObjects.MaxY; y++)
            for (int x = 0; x < NextObjects.MaxX; x++)
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
    protected IEnumerator<bool> _RemoveLinesAnimation(int[] lines)
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
    }
}