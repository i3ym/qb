using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class FieldController : MonoBehaviour
{
    public static Camera Camera;
    public static bool BlindRendering = false;

    public static Dictionary<Shapes, bool[, ]> ShapeObjects = new Dictionary<Shapes, bool[, ]>();
    protected static int[][, ] TetrominoWallKickData, TetrominoWallKickDataI; //0R R0 R2 2R 2L L2 L0 0L

    public int MaxX, MaxY;

    public int Lvl = 0;
    protected ITetrisControlsProvider Controls;
    protected int StartLvl;
    protected bool IsLvlReached, IsPaused;
    protected GameMatrix GameMatrix, CurrentObject;
    protected Shapes CurrentShape, NextShape;
    protected int CurrentRotation, CurrentX, CurrentY;
    protected float MoveDownSpeed;
    protected List<Shapes> RandomBag = new List<Shapes>();

    protected float FastMoveDownSpeed => MoveDownSpeed > 2f / 60f ? 2f / 60f : MoveDownSpeed / 2f;
    protected int Score { get => _score; set { _score = value; ScoreText.text = value.ToString(); } }
    protected int Lines { get => _lines; set { _lines = value; LinesText.text = value.ToString(); } }
    protected int _score, _lines;

    [SerializeField]
    protected TextMeshProUGUI ScoreText = null, TimeText = null, LvlText = null, LinesText = null;
    [SerializeField]
    protected FieldRendererBase FieldRenderer = null;

    static FieldController()
    {
        ShapeObjects.Clear();
        ShapeObjects.Add(Shapes.G, new bool[, ] { { false, true, false }, { false, true, false }, { false, true, true } });
        ShapeObjects.Add(Shapes.GR, new bool[, ] { { false, true, true }, { false, true, false }, { false, true, false } });
        ShapeObjects.Add(Shapes.I, new bool[, ] { { false, true, false, false }, { false, true, false, false }, { false, true, false, false }, { false, true, false, false } });
        ShapeObjects.Add(Shapes.O, new bool[, ] { { true, true }, { true, true } });
        ShapeObjects.Add(Shapes.T, new bool[, ] { { false, true, false }, { false, true, true }, { false, true, false } });
        ShapeObjects.Add(Shapes.Z, new bool[, ] { { true, false, false }, { true, true, false }, { false, true, false } });
        ShapeObjects.Add(Shapes.ZR, new bool[, ] { { false, true, false }, { true, true, false }, { true, false, false } });

        TetrominoWallKickData = new int[4][, ];
        TetrominoWallKickData[0] = new int[, ] { { 0, 0 }, {-1, 0 }, {-1, 1 }, { 0, -2 }, {-1, -2 } };
        TetrominoWallKickData[1] = new int[, ] { { 0, 0 }, { 1, 0 }, { 1, -1 }, { 0, 2 }, { 1, 2 } };
        TetrominoWallKickData[2] = new int[, ] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 0, -2 }, { 1, 2 } };
        TetrominoWallKickData[3] = new int[, ] { { 0, 0 }, {-1, 0 }, {-1, -1 }, { 0, 2 }, {-1, 2 } };

        TetrominoWallKickDataI = new int[4][, ];
        TetrominoWallKickDataI[0] = new int[, ] { { 0, 0 }, {-2, 0 }, { 1, 0 }, {-2, -1 }, { 1, 2 } };
        TetrominoWallKickDataI[1] = new int[, ] { { 0, 0 }, {-1, 0 }, { 2, 0 }, {-1, 2 }, { 2, -1 } };
        TetrominoWallKickDataI[2] = new int[, ] { { 0, 0 }, { 2, 0 }, {-1, 0 }, { 2, 1 }, {-1, -2 } };
        TetrominoWallKickDataI[3] = new int[, ] { { 0, 0 }, { 1, 0 }, {-2, 0 }, { 1, -2 }, {-2, 1 } };
    }

    protected virtual void Awake()
    {
        Camera = Camera.main;

        if (FieldRenderer == null) FieldRenderer = GetComponent<FieldRendererBase>();

        FieldRenderer.Initialize();
        FieldRenderer.SetMaxs(GetMaxX(), GetMaxY());

        SetDefaults();
    }

    public void SetDefaults()
    {
        MaxX = GetMaxX();
        MaxY = GetMaxY();

        IsPaused = true;
        IsLvlReached = false;
        GameMatrix = new GameMatrix(MaxX, MaxY);
        CurrentObject = new GameMatrix(0, 0);
        CurrentRotation = 0;
        CurrentX = MaxX / 2 - 1;
        CurrentY = MaxY - 1;
        RandomBag.Clear();
        Score = 0;
        Lines = 0;
        TimeText.text = new DateTime(0).ToLongTimeString();
        NextShape = (Shapes) new System.Random().Next(7);
        SetLvl(Lvl);

        FieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);
    }
    public void StartGame()
    {
        StopAllCoroutines();
        SetDefaults();
        IsPaused = false;

        StartCoroutine(MoveBlockVerticalCoroutine());
        StartCoroutine(MoveHorizontalCoroutine());
        StartCoroutine(BlockRotateCoroutine());
        StartCoroutine(TimeTextUpdateCoroutine());

        SpawnBlock();
    }

    //

    protected IEnumerator RemoveLineAnimation(List<int> y)
    {
        CurrentX = 4;
        CurrentY = 25;
        IsPaused = true;

        yield return new WaitForSeconds(FieldRenderer.RemoveLinesAnimation(y, GameMatrix, CurrentObject, CurrentX, CurrentY));

        for (int i = y.Count - 1; i > -1; i--)
            for (int y2 = y[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    GameMatrix[x2, y2] = GameMatrix[x2, y2 + 1];

        for (int x2 = 0; x2 < MaxX; x2++)
            GameMatrix[x2, MaxY - 1] = false;

        FieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);

        IsPaused = false;
        SpawnBlock();
    }
    protected IEnumerator TimeTextUpdateCoroutine()
    {
        WaitForSeconds wff = new WaitForSeconds(1f);

        var time = new DateTime(0);

        while (true)
        {
            while (IsPaused) yield return new WaitWhile(() => IsPaused);

            TimeText.text = time.ToLongTimeString();

            yield return wff;
            time = time.AddSeconds(1);
        }
    }
    protected IEnumerator BlockRotateCoroutine()
    {
        bool rotate = false;

        while (true)
        {
            while (IsPaused) yield return new WaitWhile(() => IsPaused);

            if (Controls.Rotate && !rotate) RotateBlock(true);
            else if (Controls.RotateC && !rotate) RotateBlock(false);

            rotate = Controls.Rotate || Controls.RotateC;

            yield return null;
        }
    }
    protected IEnumerator MoveHorizontalCoroutine()
    {
        const float firstPause = 6f / 60f;
        const float nextPause = 3f / 60f;
        WaitForSeconds firstw = new WaitForSeconds(firstPause);
        WaitForSeconds nextw = new WaitForSeconds(nextPause);

        bool isStarted = false;
        float time;
        int horizontal;

        while (true)
        {
            while (IsPaused) yield return new WaitWhile(() => IsPaused);

            horizontal = Controls.Horizontal;

            if (horizontal != 0)
            {
                MoveHorizontal(horizontal);

                time = Time.time;
                while (time > Time.time - (isStarted ? nextPause : firstPause))
                {
                    yield return null;
                    if (horizontal == 0) break;
                }

                if (!isStarted) isStarted = true;
            }
            else isStarted = false;

            yield return null;
        }
    }
    protected IEnumerator MoveBlockVerticalCoroutine()
    {
        bool needStop()
        {
            for (int x = 0; x < CurrentObject.MaxX; x++)
                for (int y = 0; y < CurrentObject.MaxY; y++)
                {
                    if (x + CurrentX >= GameMatrix.MaxX ||
                        y + CurrentY - 1 >= GameMatrix.MaxY ||
                        x + CurrentX < 0) continue;

                    if (CurrentObject[x, y] && (y + CurrentY - 1 < 0 || GameMatrix[x + CurrentX, y + CurrentY - 1]))
                        return true;
                }

            return false;
        }


        float time;
        float speed = MoveDownSpeed;

        while (true)
        {
            while (IsPaused) yield return new WaitWhile(() => IsPaused);

            time = Time.time;

            while (time + (Controls.Down ? FastMoveDownSpeed : MoveDownSpeed) > Time.time)
                yield return null;

            if (IsPaused) continue;
            if (needStop())
            {
                SetBlockToField();
                RemoveLines();
            }
            else
            {
                CurrentY--;
                Score++;
            }

            if (IsPaused) continue;

            FieldRenderer.UpdateMatrixFast(GameMatrix, CurrentObject, CurrentX, CurrentY);
        }
    }

    protected void SpawnBlock()
    {
        if (RandomBag.Count == 0)
            RandomBag.AddRange(new Shapes[] { Shapes.G, Shapes.GR, Shapes.I, Shapes.O, Shapes.T, Shapes.Z, Shapes.ZR });

        CurrentX = 4;
        CurrentY = 19;
        CurrentRotation = 0;
        CurrentObject = ShapeObjects[CurrentShape = NextShape];

        int take = new System.Random().Next(RandomBag.Count);
        NextShape = RandomBag[take];
        RandomBag.RemoveAt(take);
        FieldRenderer.ShowNextShape(NextShape);

        for (int x = 0; x < CurrentObject.MaxX; x++)
            for (int y = 0; y < CurrentObject.MaxY; y++)
                if (CurrentObject[x, y] && GameMatrix[x + CurrentX, y + CurrentY])
                {
                    GameOver();
                    return;
                }

        FieldRenderer.UpdateColors(GameMatrix, CurrentShape);
        FieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);
    }
    protected void SetBlockToField()
    {
        for (int x = 0; x < CurrentObject.MaxX; x++)
            for (int y = 0; y < CurrentObject.MaxY; y++)
                GameMatrix[CurrentX + x, CurrentY + y] |= CurrentObject[x, y];
    }
    protected void MoveHorizontal(int add)
    {
        for (int x = 0; x < CurrentObject.MaxX; x++)
            for (int y = 0; y < CurrentObject.MaxY; y++)
                if (CurrentObject[x, y] && (CurrentX + x + add < 0 || CurrentX + x + add >= GameMatrix.MaxX || GameMatrix[CurrentX + x + add, CurrentY + y]))
                    return;

        CurrentX += add;
        FieldRenderer.UpdateMatrixFast(GameMatrix, CurrentObject, CurrentX, CurrentY);
    }
    protected void RotateBlock(bool clockwise)
    {
        bool[, ] newObject = new bool[CurrentObject.MaxY, CurrentObject.MaxX];

        if (clockwise)
            for (int x = 0; x < CurrentObject.MaxX; x++)
                for (int y = 0; y < CurrentObject.MaxY; y++)
                    newObject[y, CurrentObject.MaxX - 1 - x] = CurrentObject[x, y];
        else
            for (int x = 0; x < CurrentObject.MaxX; x++)
                for (int y = 0; y < CurrentObject.MaxY; y++)
                    newObject[CurrentObject.MaxY - 1 - y, x] = CurrentObject[x, y];

        int cx = 0, cy = 0;
        bool rotate = false;
        int[, ] wkd;

        for (int i = 0; i < 5; i++)
        {
            wkd = (CurrentShape == Shapes.I ? TetrominoWallKickDataI : TetrominoWallKickData) [CurrentRotation];

            cx = CurrentX + (clockwise? wkd[i, 0]: -wkd[i, 0]);
            cy = CurrentY + (clockwise? wkd[i, 1]: -wkd[i, 1]);

            if (TryWallKick(cx, cy, newObject))
            {
                rotate = true;
                break;
            }
        }

        if (!rotate) return;

        CurrentX = cx;
        CurrentY = cy;
        CurrentObject = newObject;
        if (++CurrentRotation == 4) CurrentRotation = 0;

        FieldRenderer.UpdateMatrixFast(GameMatrix, CurrentObject, CurrentX, CurrentY);
    }
    protected bool TryWallKick(int cx, int cy, bool[, ] newObject)
    {
        for (int x = 0; x < newObject.GetLength(0); x++)
            for (int y = 0; y < newObject.GetLength(1); y++)
            {
                if (x + cx < 0 || x + cx >= GameMatrix.MaxX ||
                    y + cy < 0 || y + cy >= GameMatrix.MaxY ||
                    GameMatrix[x + cx, y + cy] && newObject[x, y])
                    return false;
            }

        return true;
    }
    protected void SetLvl(int lvl)
    {
        Lvl = lvl;
        LvlText.text = Lvl.ToString();

        if (Lvl == 0) MoveDownSpeed = 48f / 60f;
        else if (Lvl == 1) MoveDownSpeed = 43f / 60f;
        else if (Lvl == 2) MoveDownSpeed = 38f / 60f;
        else if (Lvl == 3) MoveDownSpeed = 33f / 60f;
        else if (Lvl == 4) MoveDownSpeed = 28f / 60f;
        else if (Lvl == 5) MoveDownSpeed = 23f / 60f;
        else if (Lvl == 6) MoveDownSpeed = 18f / 60f;
        else if (Lvl == 7) MoveDownSpeed = 13f / 60f;
        else if (Lvl == 8) MoveDownSpeed = 8f / 60f;
        else if (Lvl == 9) MoveDownSpeed = 6f / 60f;
        else if (Lvl > 10 && Lvl < 13) MoveDownSpeed = 5f / 60f;
        else if (Lvl > 12 && Lvl < 16) MoveDownSpeed = 4f / 60f;
        else if (Lvl > 15 && Lvl < 19) MoveDownSpeed = 3f / 60f;
        else if (Lvl > 18 && Lvl < 29) MoveDownSpeed = 2f / 60f;
        else if (Lvl > 28) MoveDownSpeed = 1f / 60f;
    }
    protected void RemoveLines()
    {
        List<int> indexs = new List<int>();

        for (int y = 0; y < MaxY; y++)
        {
            for (int x = 0; x < MaxX; x++)
                if (!GameMatrix[x, y])
                    goto End;

            indexs.Add(y);

            End: ;
        }

        if (indexs.Count != 0)
        {
            Lines += indexs.Count;

            if (!IsLvlReached && (Lines >= StartLvl * 10 + 10 || StartLvl * 10 >= 150)) IsLvlReached = true;
            if (IsLvlReached && (Lines - indexs.Count) % 10 + indexs.Count > 9) SetLvl(++Lvl);

            if (indexs.Count == 1) Score += (indexs[0] + 1) * 40 * Lvl;
            else if (indexs.Count == 2) Score += (indexs[0] + 1) * 100 * Lvl;
            else if (indexs.Count == 3) Score += (indexs[0] + 1) * 300 * Lvl;
            else if (indexs.Count == 4) Score += (indexs[0] + 1) * 1200 * Lvl;

            StartCoroutine(RemoveLineAnimation(indexs));
        }
        else SpawnBlock();
    }

    public virtual void GameOver()
    {
        StopAllCoroutines();
        IsPaused = true;
        Menu.SetMenu<GameOverMenu>();

        if (BlindRendering)
        {
            BlindRendering = false;
            FieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);
            BlindRendering = true;
        }
    }
    public void RestartGame()
    {
        Lvl = StartLvl;
        StartGame();
    }

    protected abstract int GetMaxX();
    protected abstract int GetMaxY();
}