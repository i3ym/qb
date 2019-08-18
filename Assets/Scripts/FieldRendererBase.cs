using System.Collections.Generic;
using UnityEngine;

public abstract class FieldRendererBase : MonoBehaviour
{
    [SerializeField]
    IFieldRendererExtension[] Extensions = new IFieldRendererExtension[] { };
    [SerializeField]
    protected GameObject FieldGameObjectPrefab = null;
    [SerializeField]
    protected Material BaseFieldGOMaterial = null;
    [SerializeField]
    protected int FieldX = 0, FieldY = 0, NextX = 0, NextY = 0;

    protected Renderer[, ] GameObjects = new Renderer[10, 20];
    protected GameObject[] NextObjects = new GameObject[7];
    protected Dictionary<Shapes, Material> ShapeColors = new Dictionary<Shapes, Material>();

    void Awake()
    {
        for (int y = 0; y < 20; y++)
            for (int x = 0; x < 10; x++)
                GameObjects[x, y] = GetNewCube(new Vector2(x - 5 + .5f, y - 10 + .5f));

        for (int i = 0; i < 7; i++) NextObjects[i] = CreateNextObject((Shapes) i);

        ShapeColors.Add(Shapes.S, GetMaterial(Color.blue));
        ShapeColors.Add(Shapes.SR, GetMaterial(new Color(1f, .5f, 0f)));
        ShapeColors.Add(Shapes.Z, GetMaterial(Color.red));
        ShapeColors.Add(Shapes.ZR, GetMaterial(Color.green));
        ShapeColors.Add(Shapes.I, GetMaterial(new Color(0f, 1f, 1f)));
        ShapeColors.Add(Shapes.T, GetMaterial(new Color(.5f, 0f, 1f)));
        ShapeColors.Add(Shapes.O, GetMaterial(Color.yellow));

        foreach (var extension in Extensions) extension.Start();
    }

    void Update()
    {
        foreach (var extension in Extensions)
            extension.Update();
    }

    //

    protected virtual Material GetMaterial(Color color) => new Material(BaseFieldGOMaterial) { color = color };
    protected virtual Renderer GetNewCube(Vector3 pos)
    {
        GameObject go = Instantiate(FieldGameObjectPrefab, pos, Quaternion.identity, transform);
        go.SetActive(false);

        return go.GetComponent<Renderer>();
    }
    protected virtual Renderer GetNewNextCube(Vector3 pos, Transform parent, Material mat)
    {
        var cube = GetNewCube(pos);
        cube.transform.SetParent(parent, false);
        cube.sharedMaterial = mat;
        return cube;
    }
    protected virtual GameObject CreateNextObject(Shapes shape)
    {
        var go = new GameObject("NEXT_" + shape.ToString());
        go.transform.position = new Vector3(NextX, NextY);

        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)
            {
                try
                {
                    if (Game.ShapeObjects[shape][x, y]) GetNewNextCube(go.transform.position + new Vector3(x, y), go.transform, ShapeColors[shape]);
                }
                catch { }
            }

        return go;
    }

    public void UpdateMatrix(bool[, ] GameMatrix, bool[, ] CurrentObject, int X, int Y)
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

                        if ((y == maxy && GameMatrix[x, y]) || CurrentObject[x - X, y - Y])
                            GameObjects[x, y]?.gameObject.SetActive(true);
                        else GameObjects[x, y]?.gameObject.SetActive(false);
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
                        if (GameMatrix[x, y] || CurrentObject[x - X, y - Y])
                            GameObjects[x, y]?.gameObject.SetActive(true);
                        else GameObjects[x, y]?.gameObject.SetActive(false);
                    }
                    catch { GameObjects[x, y]?.gameObject.SetActive(false); }
                }
            }
        }
    }
    public virtual void UpdateColors(bool[, ] GayMatrix, Shapes CurrentShape)
    {
        for (int y = 0; y < 20; y++)
            for (int x = 0; x < 10; x++)
                if (!GayMatrix[x, y]) GameObjects[x, y].sharedMaterial = ShapeColors[CurrentShape];
    }
    public virtual void ShowNextShape(Shapes shape)
    {
        for (int i = 0; i < 7; i++)
            NextObjects[i].SetActive(false);
        NextObjects[(int) shape].SetActive(true);
    }

    /// <returns> Duration of animation in seconds </returns> 
    public virtual float RemoveLinesAnimation(List<int> y, bool[, ] GayMatrix, bool[, ] CurrentObject, int X, int Y)
    {
        for (int i = y.Count - 1; i > -1; i--)
            for (int y2 = y[i]; y2 < 19; y2++)
                for (int x2 = 0; x2 < 10; x2++)
                {
                    GayMatrix[x2, y2] = GayMatrix[x2, y2 + 1];
                    GameObjects[x2, y2].sharedMaterial = GameObjects[x2, y2 + 1].sharedMaterial;
                }
       
        for (int x2 = 0; x2 < 10; x2++)
            GayMatrix[x2, 19] = false;

        UpdateMatrix(GayMatrix, CurrentObject, X, Y);
        return .1f;
    }
}