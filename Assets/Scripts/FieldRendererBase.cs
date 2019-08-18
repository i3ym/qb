using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FieldRendererBase : MonoBehaviour
{
    public abstract void UpdateMatrix(bool[, ] GameMatrix, bool[, ] CurrentObject, int X, int Y);
    public abstract void UpdateColors(bool[, ] GayMatrix, Shapes CurrentShape);
    public abstract void ShowNextShape(Shapes shape);

    /// <returns> Duration of animation in seconds </returns> 
    public abstract float RemoveLinesAnimation(List<int> y, bool[, ] GayMatrix, bool[, ] CurrentObject, int X, int Y);
}

public abstract class FieldRendererBase<T> : FieldRendererBase where T : Component
{
    [SerializeField]
    IFieldRendererExtension[] Extensions = new IFieldRendererExtension[] { };
    [SerializeField]
    protected GameObject FieldGameObjectPrefab = null;
    [SerializeField]
    protected Transform NextObjectsParent = null;
    [SerializeField]
    protected Material BaseFieldGOMaterial = null;

    protected T[, ] GameObjects = new T[10, 20];
    protected GameObject[] NextObjects = new GameObject[7];
    protected Dictionary<Shapes, Material> ShapeMaterials = new Dictionary<Shapes, Material>();

    void Awake()
    {
        ShapeMaterials.Add(Shapes.G, GetMaterial(Color.blue));
        ShapeMaterials.Add(Shapes.GR, GetMaterial(new Color(1f, .5f, 0f)));
        ShapeMaterials.Add(Shapes.Z, GetMaterial(Color.red));
        ShapeMaterials.Add(Shapes.ZR, GetMaterial(Color.green));
        ShapeMaterials.Add(Shapes.I, GetMaterial(new Color(0f, 1f, 1f)));
        ShapeMaterials.Add(Shapes.T, GetMaterial(new Color(.5f, 0f, 1f)));
        ShapeMaterials.Add(Shapes.O, GetMaterial(Color.yellow));

        for (int y = 0; y < 20; y++)
            for (int x = 0; x < 10; x++)
                GameObjects[x, y] = GetNewCube(new Vector2(x - 5, y - 10)).GetComponent<T>();

        for (int i = 0; i < 7; i++) NextObjects[i] = CreateNextObject((Shapes) i);

        foreach (var extension in Extensions) extension.Start();
    }
    void Update()
    {
        foreach (var extension in Extensions)
            extension.Update();
    }

    //

    protected abstract void SetMaterial(T t, Material material);
    protected abstract void SetMaterial(T t, T t2);

    protected virtual Material GetMaterial(Color color) => new Material(BaseFieldGOMaterial) { color = color };
    protected virtual GameObject GetNewCube(Vector3 pos)
    {
        GameObject go = Instantiate(FieldGameObjectPrefab, pos + new Vector3(.5f, .5f), Quaternion.identity, transform);
        go.SetActive(false);

        return go;
    }
    protected virtual GameObject GetNewNextCube(Vector3 pos, Transform parent, Material mat)
    {
        var cube = GetNewCube(pos);
        cube.transform.SetParent(parent, false);
        SetMaterial(cube.GetComponent<T>(), mat);
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
                    if (Game.ShapeObjects[shape][x, y]) GetNewNextCube(new Vector3(x + addx, y + addy), go.transform, ShapeMaterials[shape]).SetActive(true);
                }
                catch { }
            }

        return go;
    }

    public override void UpdateMatrix(bool[, ] GameMatrix, bool[, ] CurrentObject, int X, int Y)
    {
        if (Game.BlindRendering)
        {
            int maxy = -1;

            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    try
                    {
                        maxy = -1;

                        for (int i = 19; i > -1; i--)
                            if (GameMatrix[x, i])
                            {
                                maxy = i;
                                break;
                            }

                        GameObjects[x, y]?.gameObject.SetActive((y == maxy && GameMatrix[x, y]) || CurrentObject[x - X, y - Y]);
                    }
                    catch { GameObjects[x, y]?.gameObject.SetActive(false); }
                }
            }
        }
        else
        {
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    try
                    {
                        GameObjects[x, y]?.gameObject.SetActive(GameMatrix[x, y] || CurrentObject[x - X, y - Y]);
                    }
                    catch { GameObjects[x, y]?.gameObject.SetActive(false); }
                }
            }
        }
    }
    public override void UpdateColors(bool[, ] GayMatrix, Shapes CurrentShape)
    {
        for (int y = 0; y < 20; y++)
            for (int x = 0; x < 10; x++)
                if (!GayMatrix[x, y]) SetMaterial(GameObjects[x, y], ShapeMaterials[CurrentShape]);
    }
    public override void ShowNextShape(Shapes shape)
    {
        for (int i = 0; i < 7; i++) NextObjects[i].SetActive(false);
        NextObjects[(int) shape].SetActive(true);
    }

    public override float RemoveLinesAnimation(List<int> y, bool[, ] GayMatrix, bool[, ] CurrentObject, int X, int Y)
    {
        for (int i = y.Count - 1; i > -1; i--)
            for (int y2 = y[i]; y2 < 19; y2++)
                for (int x2 = 0; x2 < 10; x2++)
                {
                    GayMatrix[x2, y2] = GayMatrix[x2, y2 + 1];
                    SetMaterial(GameObjects[x2, y2], GameObjects[x2, y2 + 1]);
                }

        for (int x2 = 0; x2 < 10; x2++)
            GayMatrix[x2, 19] = false;

        UpdateMatrix(GayMatrix, CurrentObject, X, Y);
        return .1f;
    }
}