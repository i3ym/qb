using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FieldObjectRendererBase<T> : FieldRendererBase where T : Component
{
    [SerializeField]
    protected GameObject FieldGameObjectPrefab = null;
    [SerializeField]
    protected Transform NextObjectsParent = null;
    [SerializeField]
    protected Material BaseFieldGOMaterial = null;

    protected T[, ] GameObjects;
    protected GameObject[] NextObjects = new GameObject[7];
    protected Dictionary<Shapes, Material> ShapeMaterials = new Dictionary<Shapes, Material>();

    public override void Initialize()
    {
        base.Initialize();

        foreach (var shape in Enum.GetValues(typeof(Shapes)).Cast<Shapes>())
            ShapeMaterials.Add(shape, GetMaterial(shape));

        for (int i = 0; i < 7; i++) NextObjects[i] = CreateNextObject((Shapes) i);
    }

    public override void SetMaxs(int maxx, int maxy)
    {
        MaxX = maxx;
        MaxY = maxy;

        GameObjects = new T[maxx, maxy];

        for (int y = 0; y < maxy; y++)
            for (int x = 0; x < maxx; x++)
                GameObjects[x, y] = GetNewCube(new Vector2(x - MaxX / 2f, y - MaxY / 2f)).GetComponent<T>();
    }

    protected abstract void SetMaterial(T t, Material material);
    protected abstract void SetMaterial(T t, T t2);

    protected virtual Material GetMaterial(Shapes shape) => new Material(BaseFieldGOMaterial) { color = ShapeColors[shape] };
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
                if (!gameMatrix[x, y]) SetMaterial(GameObjects[x, y], ShapeMaterials[currentShape]);
    }
    public override void ShowNextShape(Shapes shape)
    {
        for (int i = 0; i < 7; i++) NextObjects[i].SetActive(false);
        NextObjects[(int) shape].SetActive(true);
    }

    public override float RemoveLinesAnimation(List<int> y, GameMatrix gameMatrix, GameMatrix currentObject, int X, int Y)
    {
        for (int i = y.Count - 1; i > -1; i--)
            for (int y2 = y[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    SetMaterial(GameObjects[x2, y2], GameObjects[x2, y2 + 1]);

        return .1f;
    }
}