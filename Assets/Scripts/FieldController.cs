using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class FieldController : MonoBehaviour, IFieldController
{
    public static Camera Camera;
    public static bool BlindRendering = false;

    public static Dictionary<Shapes, GameMatrix> ShapeObjects = new Dictionary<Shapes, GameMatrix>();
    protected static int[][, ] TetrominoWallKickData, TetrominoWallKickDataI; //0R R0 R2 2R 2L L2 L0 0L

    [HideInInspector]
    public int MaxX, MaxY;

    [HideInInspector]
    public int Lvl = 0;
    [HideInInspector]
    public bool IsAdContinued;
    [HideInInspector]
    public int StartLvl = 0;
    [HideInInspector]
    public bool IsPaused = false;

    protected ITetrisControlsProvider Controls;
    protected bool IsLvlReached;
    protected List<Shapes> RandomBag = new List<Shapes>();

    protected GameMatrix GameMatrix, CurrentObject;
    protected int CurrentX, CurrentY, CurrentRotation;
    protected Shapes CurrentShape, NextShape;

    protected int WaitDownTime = 0, WaitHorizontalTime = 6;
    protected State State = State.Falling;
    protected HorizontalState HorizontalState = HorizontalState.Waiting;
    protected RotateState RotateState = RotateState.Waiting;
    protected int[] LinesToRemove;
    protected IEnumerator<bool> RemoveLinesAnimationCoroutine;
    protected static int[] Levels = new int[]
    {
        52,
        48,
        44,
        40,
        36,
        32,
        27,
        21,
        16,
        10,
        9,
        8,
        7,
        6,
        5,
        5,
        4,
        3,
        3,
        2,
        2
    };
    protected int LevelSpeed => Lvl > Levels.Length ? 1 : Levels[Lvl];

    protected int Score { get => _score; set { _score = value; ScoreText.text = value.ToString(); } }
    protected int Lines { get => _lines; set { _lines = value; LinesText.text = value.ToString(); } }

    GameMatrix IFieldController.GameMatrix => GameMatrix;
    GameMatrix IFieldController.CurrentObject => CurrentObject;
    int IFieldController.MaxX => MaxX;
    int IFieldController.MaxY => MaxY;
    int IFieldController.CurrentX => CurrentX;
    int IFieldController.CurrentY => CurrentY;
    int IFieldController.CurrentRotation => CurrentRotation;
    Shapes IFieldController.CurrentShape => CurrentShape;
    Shapes IFieldController.NextShape => NextShape;

    protected int _score, _lines;

    [SerializeField]
    protected TextMeshProUGUI ScoreText = null, TimeText = null, LvlText = null, LinesText = null;
    [SerializeField]
    public FieldRendererBase FieldRenderer = null;

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

        FieldRenderer.Initialize(this);

        MaxX = GetMaxX();
        MaxY = GetMaxY();
        FieldRenderer.CreateField();

        SetDefaults();

        Time.fixedDeltaTime = 1f / 60f;
    }

    public void SetDefaults()
    {
        MaxX = GetMaxX();
        MaxY = GetMaxY();

        IsPaused = true;
        IsLvlReached = false;
        IsAdContinued = false;
        GameMatrix = new GameMatrix(MaxX, MaxY);
        CurrentObject = new GameMatrix(0, 0);
        CurrentRotation = 0;
        CurrentX = MaxX / 2 - 1;
        CurrentY = MaxY - 1;
        RandomBag.Clear();
        Score = 0;
        Lines = 0;
        WaitDownTime = 0;
        WaitHorizontalTime = 6;
        TimeText.text = new DateTime(0).ToLongTimeString();
        NextShape = (Shapes) new System.Random().Next(7);

        FieldRenderer.UpdateField();
    }
    public void StartGame()
    {
        StopAllCoroutines();
        SetDefaults();

        StartCoroutine(FrameCoroutine());
        IsPaused = false;

        SpawnBlock();
    }

    protected IEnumerator FrameCoroutine()
    {
        var wfs = new WaitForFixedUpdate();
        byte frame = 0;
        var time = new DateTime(0);

        while (true)
        {
            yield return wfs;

            if (IsPaused) continue;

            frame++;
            if (frame == 60)
            {
                time = time.AddSeconds(1);
                TimeText.text = time.ToLongTimeString();
                frame = 0;
            }

            if (State == State.Animation)
            {
                PlayAnimation();
                continue;
            }

            Rotate();
            MoveHorizontal();
            MoveDown();
        }
    }

    protected void PlayAnimation()
    {
        if (RemoveLinesAnimationCoroutine.MoveNext() && RemoveLinesAnimationCoroutine.Current) return;

        RemoveLines(LinesToRemove);

        SpawnBlock();
        WaitDownTime = 20;
        State = State.HoldingOnTop;
    }
    protected void Rotate()
    {
        var wasWaiting = State == State.WaitingOnGround;

        var rotate = PlayerControls.Rotate;
        var rotatec = PlayerControls.RotateC;

        if (RotateState == RotateState.Waiting)
        {
            if (rotate || rotatec)
            {
                if (rotate) RotateBlock(true);
                if (rotatec) RotateBlock(false);

                RotateState = RotateState.Rotated;
            }
        }
        else if (RotateState == RotateState.Rotated && !rotate && !rotatec)
            RotateState = RotateState.Waiting;

        if (wasWaiting && !NeedStop())
        {
            WaitDownTime = LevelSpeed;
            State = State.Falling;
        }
    }
    protected void MoveHorizontal()
    {
        if (WaitHorizontalTime > 0)
            WaitHorizontalTime--;

        if (WaitHorizontalTime > 0) return;

        var wasWaiting = State == State.WaitingOnGround;

        var right = PlayerControls.Right;
        var left = PlayerControls.Left;

        if (HorizontalState == HorizontalState.Waiting && (right || left))
        {
            WaitHorizontalTime = 6;
            HorizontalState = HorizontalState.Moving;
        }
        else if (HorizontalState == HorizontalState.Moving)
        {
            if (right || left) WaitHorizontalTime = 3;
            else HorizontalState = HorizontalState.Waiting;
        }

        if (right) MoveRight();
        else if (left) MoveLeft();

        if (wasWaiting && !NeedStop())
        {
            WaitDownTime = LevelSpeed;
            State = State.Falling;
        }
    }
    protected void MoveDown()
    {
        WaitDownTime--;

        if (State == State.Falling && PlayerControls.Down)
            WaitDownTime -= LevelSpeed / 3 + 1;
        else if (State == State.WaitingOnGround && PlayerControls.Down)
            WaitDownTime = 0;
        else if (State == State.HoldingOnTop && (PlayerControls.Left || PlayerControls.Right || PlayerControls.Rotate || PlayerControls.RotateC))
            WaitDownTime = 0;

        if (WaitDownTime > 0) return;

        if (State == State.HoldingOnTop)
            State = State.Falling;

        if (NeedStop())
        {
            SetBlockToField();

            LinesToRemove = GetLinesToRemove();
            if (LinesToRemove.Length == 0)
            {
                SpawnBlock();
                WaitDownTime = 20;
                State = State.HoldingOnTop;
            }
            else
            {
                RemoveLinesAnimationCoroutine = FieldRenderer.RemoveLinesAnimation(LinesToRemove);
                State = State.Animation;
            }

            return;
        }

        CurrentY--;
        FieldRenderer.UpdateFieldFast();

        WaitDownTime = LevelSpeed;

        if (NeedStop())
        {
            WaitDownTime = LevelSpeed + 20;
            State = State.WaitingOnGround;
        }
    }
    protected bool NeedStop()
    {
        for (int y = 0; y < CurrentObject.MaxY; y++)
            for (int x = 0; x < CurrentObject.MaxX; x++)
            {
                if (x + CurrentX >= GameMatrix.MaxX ||
                    y + CurrentY - 1 >= GameMatrix.MaxY ||
                    x + CurrentX < 0) continue;

                if (CurrentObject[x, y] && (y + CurrentY - 1 < 0 || GameMatrix[x + CurrentX, y + CurrentY - 1]))
                    return true;
            }

        return false;
    }

    protected void SpawnBlock()
    {
        if (RandomBag.Count == 0)
            RandomBag.AddRange(new Shapes[] { Shapes.G, Shapes.GR, Shapes.I, Shapes.O, Shapes.T, Shapes.Z, Shapes.ZR });

        CurrentRotation = 0;
        CurrentShape = NextShape;
        CurrentObject = ShapeObjects[CurrentShape];

        CurrentX = 4;
        CurrentY = 20 - CurrentObject.MaxY;

        for (int y = CurrentObject.MaxY - 1; y >= 0; y--)
        {
            for (int x = 0; x < CurrentObject.MaxX; x++)
                if (CurrentObject[x, y])
                    goto end;

            CurrentY++;
        }
        end: ;

        int take = new System.Random().Next(RandomBag.Count);
        NextShape = RandomBag[take];
        RandomBag.RemoveAt(take);
        FieldRenderer.ShowNextShape();

        for (int y = 0; y < CurrentObject.MaxY; y++)
            for (int x = 0; x < CurrentObject.MaxX; x++)
                if (CurrentObject[x, y] && GameMatrix[x + CurrentX, y + CurrentY])
                {
                    GameOver();
                    return;
                }

        FieldRenderer.UpdateColors();
        FieldRenderer.UpdateField();
    }
    protected void SetBlockToField()
    {
        for (int x = 0; x < CurrentObject.MaxX; x++)
            for (int y = 0; y < CurrentObject.MaxY; y++)
                GameMatrix[CurrentX + x, CurrentY + y] |= CurrentObject[x, y];
    }
    protected void MoveRight()
    {
        for (int y = 0; y < CurrentObject.MaxY; y++)
            for (int x = 0; x < CurrentObject.MaxX; x++)
                if (CurrentObject.GetWithoutChecks(x, y) && (CurrentX + x + 1 >= GameMatrix.MaxX || GameMatrix[CurrentX + x + 1, CurrentY + y]))
                    return;

        CurrentX++;
        FieldRenderer.UpdateFieldFast();
    }
    protected void MoveLeft()
    {
        for (int y = 0; y < CurrentObject.MaxY; y++)
            for (int x = 0; x < CurrentObject.MaxX; x++)
                if (CurrentObject.GetWithoutChecks(x, y) && (CurrentX + x - 1 < 0 || GameMatrix[CurrentX + x - 1, CurrentY + y]))
                    return;

        CurrentX--;
        FieldRenderer.UpdateFieldFast();
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

        FieldRenderer.UpdateFieldFast();
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

    protected int[] GetLinesToRemove()
    {
        var lines = new List<int>(GameMatrix.MaxY);

        for (int y = 0; y < MaxY; y++)
            if (IsLineFull(y))
                lines.Add(y);

        // TODO move somewhere
        if (lines.Count != 0)
        {
            Lines += lines.Count;

            if (!IsLvlReached && (Lines >= StartLvl * 10 + 10 || StartLvl * 10 >= 150)) IsLvlReached = true;
            if (IsLvlReached && (Lines - lines.Count) % 10 + lines.Count > 9) Lvl++;

            if (lines.Count == 1) Score += (lines[0] + 1) * 40 * Lvl;
            else if (lines.Count == 2) Score += (lines[0] + 1) * 100 * Lvl;
            else if (lines.Count == 3) Score += (lines[0] + 1) * 300 * Lvl;
            else if (lines.Count == 4) Score += (lines[0] + 1) * 1200 * Lvl;
        }
        // /TODO

        return lines.ToArray();
    }
    protected void RemoveLines(int[] lines)
    {
        for (int i = lines.Length - 1; i > -1; i--)
            for (int y2 = lines[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    GameMatrix[x2, y2] = GameMatrix[x2, y2 + 1];

        for (int x2 = 0; x2 < MaxX; x2++)
            GameMatrix[x2, MaxY - 1] = false;
    }

    protected bool IsLineFull(int y)
    {
        for (int x = 0; x < MaxX; x++)
            if (!GameMatrix[x, y])
                return false;

        return true;
    }
    protected bool IsLineEmpty(int y)
    {
        for (int x = 0; x < MaxX; x++)
            if (GameMatrix[x, y])
                return false;

        return true;
    }

    protected void RemoveTopLines(int count)
    {
        for (int y = 0; y < count; y++)
            for (int x = 0; x < GameMatrix.MaxX; x++)
                GameMatrix[x, GameMatrix.MaxY - 1 - y] = false;

        FieldRenderer.UpdateColors();
        FieldRenderer.UpdateField();
    }

    public virtual void GameOver()
    {
        IsPaused = true;
        Menu.SetMenu<GameOverMenu>();

        if (BlindRendering)
        {
            BlindRendering = false;
            FieldRenderer.UpdateField();
            BlindRendering = true;
        }
    }
    public void RestartGame()
    {
        Lvl = StartLvl;
        StartGame();
    }
    public void ContinueGameWithAd()
    {
        if (IsAdContinued)
        {
            RestartGame();
            return;
        }

        IsAdContinued = true;
        IsPaused = false;
        RemoveTopLines(7);
    }

    protected abstract int GetMaxX();
    protected abstract int GetMaxY();
}

public enum State : byte
{
    Falling,
    WaitingOnGround,
    HoldingOnTop,
    Animation
}

public enum HorizontalState : byte
{
    Waiting,
    Moving
}

public enum RotateState : byte
{
    Waiting,
    Rotated
}