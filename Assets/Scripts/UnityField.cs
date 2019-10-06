using System;
using System.Collections.Generic;
using qb.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnityField : MonoBehaviour, IField
{
    static Dictionary<CubeTile, Rect> Uvs = new Dictionary<CubeTile, Rect>();
    static Texture2D Atlas => Config.Instance.Atlas;

    [SerializeField]
    int MaxX, MaxY;
    [SerializeField]
    public GameObject FieldGameObjectPrefab = null;
    [SerializeField]
    public Transform NextObjectsParent = null;
    [SerializeField]
    TextMeshProUGUI LevelText = null, ScoreText = null, LinesText = null, TimeText = null;

    public GameMatrix<RawImage> GameObjects { get; private set; }
    public GameMatrix<RawImage> NextObjects { get; private set; }

    static Color Transparent, White;

    static UnityField()
    {
        Config.OnSkinChange += () => RecreateUvs();

        Transparent = new Color(0, 0, 0, 0);
        White = new Color(1, 1, 1, 1);
    }
    void Awake()
    {
        Config.SelectedSkin = Config.SelectedSkin;
        CreateField();
    }

    void CreateField()
    {
        if (GameObjects != null)
            foreach (var img in GameObjects)
                GameObject.Destroy(img.gameObject);

        if (NextObjects != null)
            foreach (var img in NextObjects)
                GameObject.Destroy(img.gameObject);

        GameObjects = new RawImage[MaxX, MaxY];
        NextObjects = new RawImage[4, 4];

        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                GameObjects[x, y] = GetNewCube(new Vector2(x, y)).GetComponent<RawImage>();

        GameObject cube;
        Vector2 pos;
        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
            {
                pos = new Vector2(x, y);
                cube = GetNewCube(pos);
                cube.transform.SetParent(NextObjectsParent, false);
                (cube.transform as RectTransform).anchoredPosition = pos;

                NextObjects[x, y] = cube.GetComponent<RawImage>();
            }
    }
    public void ResetPositions()
    {
        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                GameObjects[x, y].rectTransform.anchoredPosition = new Vector2(x, y);
    }
    GameObject GetNewCube(Vector2 pos)
    {
        GameObject go = GameObject.Instantiate(FieldGameObjectPrefab, pos, Quaternion.identity, transform);
        (go.transform as RectTransform).pivot = (go.transform as RectTransform).anchorMax = (go.transform as RectTransform).anchorMin = Vector2.zero;
        (go.transform as RectTransform).anchoredPosition = pos;
        go.transform.localPosition -= new Vector3(0f, 0f, go.transform.localPosition.z);

        go.GetComponent<RawImage>().texture = Atlas;

        return go;
    }

    static void RecreateUvs()
    {
        Uvs.Clear();

        var tiles = Enum.GetValues(typeof(CubeTile));

        for (int i = 0; i < tiles.Length; i++)
            Uvs.Add((CubeTile) tiles.GetValue(i), new Rect(16f / Atlas.width * i,
                1f - (16f / Atlas.height * (Config.SelectedSkin + 1)),
                16f / Atlas.width,
                16f / Atlas.height));

        Uvs[CubeTile.None] = new Rect(0, 0, 0, 0);
    }

    public CubeTile this [int x, int y]
    {
        set => SetTile(GameObjects[x, y], value);
    }

    void SetTile(RawImage obj, CubeTile tile)
    {
        if (tile == CubeTile.None) obj.color = Transparent;
        else
        {
            obj.color = White;
            obj.uvRect = Uvs[tile];
        }
    }

    public void SetNextTile(int x, int y, CubeTile tile) => SetTile(NextObjects[x, y], tile);

    public void UpdateLevelText(int level) => LevelText.text = level.ToString();
    public void UpdateScoreText(int score) => ScoreText.text = score.ToString();
    public void UpdateLinesText(int lines) => LinesText.text = lines.ToString();
    public void UpdateTimeText(DateTime time) => TimeText.text = time.ToLongTimeString();
}