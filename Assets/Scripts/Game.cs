using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Camera Camera;
    public static bool BlindRendering = false;

    public static Dictionary<Shapes, bool[, ]> ShapeObjects = new Dictionary<Shapes, bool[, ]>();
    protected static int[][, ] TetrominoWallKickData = new int[8][, ]; //0R R0 R2 2R 2L L2 L0 0L // TODO mb remove anti-clockwise?    
    protected static int[][, ] TetrominoWallKickDataI = new int[8][, ];

    public static int Lvl = 0;
    protected int StartLvl;
    protected bool IsLvlReached = false;
    protected bool[, ] GameMatrix = new bool[10, 20];
    protected bool[, ] CurrentObject;
    protected Shapes CurrentShape, NextShape;
    protected int CurrentRotation = 0, CurrentX = 0, CurrentY = 19;
    protected float MoveSpeed, FastMoveSpeed = 2f / 60f;
    protected Coroutine MoveBlockCoroutineVariable;
    protected List<Shapes> RandomBag = new List<Shapes>();
    protected bool isPaused = false, isDead = false;
    protected int _score = 0, _lines = 0;
    protected int Score { get { return _score; } set { _score = value; ScoreText.text = value.ToString(); } }
    protected int Lines { get { return _lines; } set { _lines = value; LinesText.text = value.ToString(); } }

    [SerializeField]
    protected TextMeshPro ScoreText, TimeText, LvlText, LinesText;
    [SerializeField]
    protected Canvas canvasDead;
    [SerializeField]
    protected FieldRendererBase fieldRenderer;

    void Awake()
    {
        Camera = Camera.main;

        #region AddArrays

        ShapeObjects.Add(Shapes.G, new bool[, ] { { false, true, false }, { false, true, false }, { false, true, true } });
        ShapeObjects.Add(Shapes.GR, new bool[, ] { { false, true, true }, { false, true, false }, { false, true, false } });
        ShapeObjects.Add(Shapes.I, new bool[, ] { { false, true, false, false }, { false, true, false, false }, { false, true, false, false }, { false, true, false, false } });
        ShapeObjects.Add(Shapes.O, new bool[, ] { { true, true }, { true, true } });
        ShapeObjects.Add(Shapes.T, new bool[, ] { { false, true, false }, { false, true, true }, { false, true, false } });
        ShapeObjects.Add(Shapes.Z, new bool[, ] { { true, false, false }, { true, true, false }, { false, true, false } });
        ShapeObjects.Add(Shapes.ZR, new bool[, ] { { false, true, false }, { true, true, false }, { true, false, false } });

        TetrominoWallKickData[0] = new int[, ] { { 0, 0 }, {-1, 0 }, {-1, 1 }, { 0, -2 }, {-1, -2 } };
        TetrominoWallKickData[1] = new int[, ] { { 0, 0 }, { 1, 0 }, { 1, -1 }, { 0, 2 }, { 1, 2 } };
        TetrominoWallKickData[2] = new int[, ] { { 0, 0 }, { 1, 0 }, { 1, -1 }, { 0, 2 }, { 1, 2 } };
        TetrominoWallKickData[3] = new int[, ] { { 0, 0 }, {-1, 0 }, {-1, 1 }, { 0, -2 }, {-1, -2 } };
        TetrominoWallKickData[4] = new int[, ] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 0, -2 }, { 1, 2 } };
        TetrominoWallKickData[5] = new int[, ] { { 0, 0 }, {-1, 0 }, {-1, -1 }, { 0, 2 }, {-1, 2 } };
        TetrominoWallKickData[6] = new int[, ] { { 0, 0 }, {-1, 0 }, {-1, -1 }, { 0, 2 }, {-1, 2 } };
        TetrominoWallKickData[7] = new int[, ] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 0, -2 }, { 1, 2 } };

        TetrominoWallKickDataI[0] = new int[, ] { { 0, 0 }, {-2, 0 }, { 1, 0 }, {-2, -1 }, { 1, 2 } };
        TetrominoWallKickDataI[1] = new int[, ] { { 0, 0 }, { 2, 0 }, {-1, 0 }, { 2, 1 }, {-1, -2 } };
        TetrominoWallKickDataI[2] = new int[, ] { { 0, 0 }, {-1, 0 }, { 2, 0 }, {-1, 2 }, { 2, -1 } };
        TetrominoWallKickDataI[3] = new int[, ] { { 0, 0 }, { 1, 0 }, {-2, 0 }, { 1, -2 }, {-2, 1 } };
        TetrominoWallKickDataI[4] = new int[, ] { { 0, 0 }, { 2, 0 }, {-1, 0 }, { 2, 1 }, {-1, -2 } };
        TetrominoWallKickDataI[5] = new int[, ] { { 0, 0 }, {-2, 0 }, { 1, 0 }, {-2, -1 }, { 1, 2 } };
        TetrominoWallKickDataI[6] = new int[, ] { { 0, 0 }, { 1, 0 }, {-2, 0 }, { 1, -2 }, {-2, 1 } };
        TetrominoWallKickDataI[7] = new int[, ] { { 0, 0 }, {-1, 0 }, { 2, 0 }, {-1, 2 }, { 2, 1 } };

        #endregion
    }
    void Start()
    {
        LvlText.text = Lvl.ToString();
        SetLvl(Lvl);
        StartLvl = Lvl;

        NextShape = (Shapes) new System.Random().Next(7);
        if (fieldRenderer == null) fieldRenderer = GetComponent<FieldRendererBase>();

        fieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);

        StartCoroutine(MoveBlockSpeedCoroutine());
        StartCoroutine(MoveBlockHorizontalCoroutine());
        StartCoroutine(BlockRotateCoroutine());
        StartCoroutine(TimeTextUpdateCoroutine());

        SpawnBlock();
    }
    protected virtual void Update() { }

    IEnumerator RemoveLineAnimation(List<int> y)
    {
        CurrentX = 4;
        CurrentY = 25;
        isPaused = true;

        yield return new WaitForSeconds(fieldRenderer.RemoveLinesAnimation(y, GameMatrix, CurrentObject, CurrentX, CurrentY));

        isPaused = false;
        SpawnBlock();
    }
    IEnumerator TimeTextUpdateCoroutine()
    {
        WaitForSeconds wff = new WaitForSeconds(1f);
        TimeSpan temp = new TimeSpan();
        TimeSpan add = new TimeSpan(0, 0, 1);
        TimeText.text = temp.Hours + ":" + temp.Minutes + ":" + temp.Seconds;

        while (true)
        {
            while (isPaused) yield return null;
            yield return wff;

            temp = temp.Add(add);
            TimeText.text = temp.Hours + ":" + temp.Minutes + ":" + temp.Seconds;
        }
    }
    IEnumerator BlockRotateCoroutine()
    {
        while (true)
        {
            while (isPaused) yield return null;

            if (Input.GetKeyDown(Config.KeyRotate)) RotateBlock();

            yield return null;
        }
    }
    IEnumerator MoveBlockHorizontalCoroutine()
    {
        const float firstPause = 16f / 60f;
        const float nextPause = 6f / 60f;
        WaitForSeconds firstw = new WaitForSeconds(firstPause);
        WaitForSeconds nextw = new WaitForSeconds(nextPause);

        bool isStarted = false;
        float time;

        while (true)
        {
            while (isPaused) yield return null;

            if ((int) Input.GetAxisRaw("Horizontal") != 0)
            {
                MoveHorizontal((int) Input.GetAxisRaw("Horizontal"));

                if (isStarted)
                {
                    time = Time.time;
                    while (time > Time.time - nextPause)
                    {
                        yield return null;

                        if ((int) Input.GetAxisRaw("Horizontal") == 0)
                        {
                            StartCoroutine(MoveBlockHorizontalCoroutine());
                            yield break;
                        }
                    }
                }
                else
                {
                    time = Time.time;
                    while (time > Time.time - firstPause)
                    {
                        yield return null;

                        if ((int) Input.GetAxisRaw("Horizontal") == 0)
                        {
                            StartCoroutine(MoveBlockHorizontalCoroutine());
                            yield break;
                        }
                    }
                }

                if (!isStarted) isStarted = true;
            }
            else isStarted = false;

            yield return null;
        }
    }
    IEnumerator MoveBlockCoroutine(float speed)
    {
        bool isStop;
        fieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);

        while (true)
        {
            while (isPaused) yield return null;

            if (speed == FastMoveSpeed) Score++;
            yield return new WaitForSeconds(speed);

            isStop = false;

            for (int x = 0; x < CurrentObject.GetLength(0); x++)
                for (int y = 0; y < CurrentObject.GetLength(1); y++)
                {
                    try
                    {
                        if (CurrentObject[x, y] && (y + CurrentY - 1 < 0 || GameMatrix[x + CurrentX, y + CurrentY - 1]))
                            isStop = true;
                    }
                    catch { }
                }

            if (isStop) yield return new WaitForSeconds(speed);

            if (isPaused) continue;
            isStop = false;

            for (int x = 0; x < CurrentObject.GetLength(0); x++)
                for (int y = 0; y < CurrentObject.GetLength(1); y++)
                {
                    try
                    {
                        if (CurrentObject[x, y] && (y + CurrentY - 1 < 0 || GameMatrix[x + CurrentX, y + CurrentY - 1]))
                            isStop = true;
                    }
                    catch { }
                }

            if (isStop)
            {
                SetBlockToField();
                RemoveLines();
            }

            CurrentY--;
            fieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);
        }
    }
    IEnumerator MoveBlockSpeedCoroutine()
    {
        MoveBlockCoroutineVariable = StartCoroutine(MoveBlockCoroutine(MoveSpeed));

        while (true)
        {
            while (isPaused) yield return null;

            if (Input.GetKeyDown(Config.KeyDown))
            {
                StopCoroutine(MoveBlockCoroutineVariable);
                MoveBlockCoroutineVariable = StartCoroutine(MoveBlockCoroutine(FastMoveSpeed));
            }
            else if (Input.GetKeyUp(Config.KeyDown))
            {
                StopCoroutine(MoveBlockCoroutineVariable);
                MoveBlockCoroutineVariable = StartCoroutine(MoveBlockCoroutine(MoveSpeed));
            }

            yield return null;
        }
    }

    void SpawnBlock()
    {
        if (RandomBag.Count == 0)
            RandomBag.AddRange(new Shapes[] { Shapes.G, Shapes.GR, Shapes.I, Shapes.O, Shapes.T, Shapes.Z, Shapes.ZR });

        CurrentX = 4;
        CurrentY = 20;
        CurrentRotation = 0;

        CurrentObject = ShapeObjects[CurrentShape = NextShape];

        int take = new System.Random().Next(RandomBag.Count);
        NextShape = RandomBag[take];
        RandomBag.RemoveAt(take);
        fieldRenderer.ShowNextShape(NextShape);

        for (int x = 0; x < CurrentObject.GetLength(0); x++)
            for (int y = 0; y < CurrentObject.GetLength(1); y++)
            {
                try
                {
                    if (CurrentObject[x, y] && GameMatrix[x + 4, y + 19]) GameOver();
                }
                catch { }
            }

        StopCoroutine(MoveBlockCoroutineVariable);
        MoveBlockCoroutineVariable = StartCoroutine(MoveBlockCoroutine(MoveSpeed));

        fieldRenderer.UpdateColors(GameMatrix, CurrentShape);
        fieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);
    }
    void SetBlockToField()
    {
        for (int x = 0; x < CurrentObject.GetLength(0); x++)
            for (int y = 0; y < CurrentObject.GetLength(1); y++)
            {
                try
                {
                    if (CurrentY + y > 19)
                    {
                        GameOver();
                        return;
                    }

                    GameMatrix[CurrentX + x, CurrentY + y] = GameMatrix[CurrentX + x, CurrentY + y] || CurrentObject[x, y];
                }
                catch { }
            }

        // TODO ????? fieldRenderer.OnBlockPlace(GameMatrix);
    }
    void MoveHorizontal(int add)
    {
        for (int x = 0; x < CurrentObject.GetLength(0); x++)
            for (int y = 0; y < CurrentObject.GetLength(1); y++)
                try
                {
                    if (CurrentObject[x, y] && (CurrentX + x + add < 0 || CurrentX + x + add > 9 || GameMatrix[CurrentX + x + add, CurrentY + y])) return;
                }
        catch { }

        CurrentX += add;
        fieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);
    }
    void RotateBlock()
    {
        bool[, ] newObject = new bool[CurrentObject.GetLength(1), CurrentObject.GetLength(0)];

        for (int x = 0; x < CurrentObject.GetLength(0); x++)
            for (int y = 0; y < CurrentObject.GetLength(1); y++)
                newObject[y, CurrentObject.GetLength(0) - 1 - x] = CurrentObject[x, y];

        int cx = 0;
        int cy = 0;
        bool rotate = false;

        if (CurrentShape == Shapes.I)
        {
            for (int i = 0; i < 5; i++)
            {
                cx = CurrentX + TetrominoWallKickData[CurrentRotation * 2][i, 0];
                cy = CurrentY + TetrominoWallKickData[CurrentRotation * 2][i, 1];

                if (TryWallKick(cx, cy, newObject))
                {
                    rotate = true;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                cx = CurrentX + TetrominoWallKickData[CurrentRotation * 2][i, 0];
                cy = CurrentY + TetrominoWallKickData[CurrentRotation * 2][i, 1];

                if (TryWallKick(cx, cy, newObject))
                {
                    rotate = true;
                    break;
                }
            }
        }

        if (!rotate) return;

        CurrentX = cx;
        CurrentY = cy;
        CurrentObject = newObject;
        if (++CurrentRotation == 4) CurrentRotation = 0;

        fieldRenderer.UpdateMatrix(GameMatrix, CurrentObject, CurrentX, CurrentY);
    }
    bool TryWallKick(int cx, int cy, bool[, ] newObject)
    {
        for (int x = 0; x < newObject.GetLength(0); x++)
            for (int y = 0; y < newObject.GetLength(1); y++)
                try
                {
                    if (GameMatrix[x + cx, y + cy] && newObject[x, y]) return false;
                }
        catch { return false; }

        return true;
    }
    void SetLvl(int lvl)
    {
        Lvl = lvl;
        LvlText.text = Lvl.ToString();

        if (Lvl == 0) MoveSpeed = 48f / 60f;
        else if (Lvl == 1) MoveSpeed = 43f / 60f;
        else if (Lvl == 2) MoveSpeed = 38f / 60f;
        else if (Lvl == 3) MoveSpeed = 33f / 60f;
        else if (Lvl == 4) MoveSpeed = 28f / 60f;
        else if (Lvl == 5) MoveSpeed = 23f / 60f;
        else if (Lvl == 6) MoveSpeed = 18f / 60f;
        else if (Lvl == 7) MoveSpeed = 13f / 60f;
        else if (Lvl == 8) MoveSpeed = 8f / 60f;
        else if (Lvl == 9) MoveSpeed = 6f / 60f;
        else if (Lvl > 10 && Lvl < 13) MoveSpeed = 5f / 60f;
        else if (Lvl > 12 && Lvl < 16) MoveSpeed = 4f / 60f;
        else if (Lvl > 15 && Lvl < 19) MoveSpeed = 3f / 60f;
        else if (Lvl > 18 && Lvl < 29) MoveSpeed = 2f / 60f;
        else if (Lvl > 28) MoveSpeed = 1f / 60f;
    }

    void RemoveLines()
    {
        bool removeLine = true;
        List<int> indexs = new List<int>();

        for (int y = 0; y < 20; y++)
        {
            removeLine = true;

            for (int x = 0; x < 10; x++)
                if (!GameMatrix[x, y])
                {
                    removeLine = false;
                    break;
                }

            if (removeLine) indexs.Add(y);
        }

        if (indexs.Count != 0)
        {
            Lines += indexs.Count;

            if (!IsLvlReached && (Lines >= StartLvl * 10 + 10 || StartLvl * 10 >= 150)) IsLvlReached = true;
            if (IsLvlReached && (Lines - indexs.Count) % 10 + indexs.Count > 9) SetLvl(++Lvl);

            if (indexs.Count == 1) Score += (indexs[0] + 1) * 40 * Game.Lvl;
            else if (indexs.Count == 2) Score += (indexs[0] + 1) * 100 * Game.Lvl;
            else if (indexs.Count == 3) Score += (indexs[0] + 1) * 300 * Game.Lvl;
            else if (indexs.Count == 4) Score += (indexs[0] + 1) * 1200 * Game.Lvl;

            StartCoroutine(RemoveLineAnimation(indexs));
        }
        else SpawnBlock();
    }
    void GameOver()
    {
        isPaused = true;
        isDead = true;
        canvasDead.gameObject.SetActive(true);
        StopAllCoroutines();
    }
}