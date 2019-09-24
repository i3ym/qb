using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class FieldController : MonoBehaviour, IFieldController
{
    public static Camera Camera;
    public static bool BlindRendering = false;

    public static Dictionary<Shapes, GameMatrix<bool>> ShapeObjects = new Dictionary<Shapes, GameMatrix<bool>>();
    protected static(int, int) [][] TetrominoWallKickData, TetrominoWallKickDataI; //0R R0 R2 2R 2L L2 L0 0L

    [HideInInspector]
    public int MaxX, MaxY;

    [HideInInspector]
    public bool IsAdContinued;
    [HideInInspector]
    public int StartLvl = 0;
    [HideInInspector]
    public bool IsPaused = false;

    protected ITetrisControlsProvider Controls;
    protected bool IsLvlReached;
    protected List<Shapes> RandomBag = new List<Shapes>();

    protected GameMatrix<bool> GameMatrix, CurrentObject;
    protected int CurrentX, CurrentY, CurrentRotation;
    protected Shapes CurrentShape, NextShape;

    protected int WaitDownTime = 0, WaitHorizontalTime = 6;
    protected State State = State.Falling;
    protected HorizontalState HorizontalState = HorizontalState.Waiting;
    protected RotateState RotateState = RotateState.Waiting;
    protected int[] LinesToRemove;
    protected IEnumerator<bool> RemoveLinesAnimationCoroutine, GameOverAnimationCoroutine;
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

    protected int Score { get => _Score; set { _Score = value; ScoreText.text = value.ToString(); } }
    protected int Lines { get => _Lines; set { _Lines = value; LinesText.text = value.ToString(); } }
    protected int Lvl { get => _Lvl; set { _Lvl = value; LvlText.text = value.ToString(); } }

    GameMatrix<bool> IFieldController.GameMatrix => GameMatrix;
    GameMatrix<bool> IFieldController.CurrentObject => CurrentObject;
    int IFieldController.MaxX => MaxX;
    int IFieldController.MaxY => MaxY;
    int IFieldController.CurrentX => CurrentX;
    int IFieldController.CurrentY => CurrentY;
    int IFieldController.CurrentRotation => CurrentRotation;
    Shapes IFieldController.CurrentShape => CurrentShape;
    Shapes IFieldController.NextShape => NextShape;

    protected int _Score, _Lines, _Lvl;

    [SerializeField]
    protected TextMeshProUGUI ScoreText = null, TimeText = null, LvlText = null, LinesText = null;
    [SerializeField]
    public FieldRendererBase FieldRenderer = null;

    static FieldController()
    {
        bool[, ] convert(string text)
        {
            var spt = text.Replace(" ", "").Replace(" ", "").Replace("\r", "").Split('\n');
            bool[, ] output = new bool[spt[0].Length, spt.Length];

            for (int y = 0; y < spt.Length; y++)
                for (int x = 0; x < spt[y].Length; x++)
                    output[x, y] = spt[spt[y].Length - 1 - y][x] == '1';

            return output;
        }

        ShapeObjects.Clear();
        ShapeObjects.Add(Shapes.G, convert(
            @"001
            111
            000"));
        ShapeObjects.Add(Shapes.GR, convert(
            @"100
            111
            000"));
        ShapeObjects.Add(Shapes.I, convert(
            @"0000
            1111
            0000
            0000"));
        ShapeObjects.Add(Shapes.O, convert(
            @"11
            11"));
        ShapeObjects.Add(Shapes.T, convert(
            @"010
            111
            000"));
        ShapeObjects.Add(Shapes.Z, convert(
            @"110
            011
            000"));
        ShapeObjects.Add(Shapes.ZR, convert(
            @"011
            110
            000"));

        #region WallKickData

        TetrominoWallKickData = new(int, int) [4][];
        TetrominoWallKickData[0] = new []
        {
            (0, 0), (-1, 0), (-1, 1), (0, -2), (-1, -2)
        };
        TetrominoWallKickData[1] = new []
        {
            (0, 0), (1, 0), (1, -1), (0, 2), (1, 2)
        };
        TetrominoWallKickData[2] = new []
        {
            (0, 0), (1, 0), (1, 1), (0, -2), (1, -2)
        };
        TetrominoWallKickData[3] = new []
        {
            (0, 0), (-1, 0), (-1, -1), (0, 2), (-1, 2)
        };

        TetrominoWallKickDataI = new(int, int) [4][];
        TetrominoWallKickDataI[0] = new []
        {
            (0, 0), (-2, 0), (1, 0), (-2, -1), (1, 2)
        };
        TetrominoWallKickDataI[1] = new []
        {
            (0, 0), (-1, 0), (2, 0), (-1, 2), (2, -1)
        };
        TetrominoWallKickDataI[2] = new []
        {
            (0, 0), (2, 0), (-1, 0), (2, 1), (-1, -2)
        };
        TetrominoWallKickDataI[3] = new []
        {
            (0, 0), (1, 0), (-2, 0), (1, -2), (-2, 1)
        };

        #endregion
    }

    protected virtual void Awake()
    {
        Camera = Camera.main;

        if (FieldRenderer == null) FieldRenderer = GetComponent<FieldRendererBase>();

        MaxX = GetMaxX();
        MaxY = GetMaxY();
        FieldRenderer.Initialize(this);
        FieldRenderer.CreateField();

        SetDefaults();

        Time.fixedDeltaTime = 1f / 60f;
    }

    public void SetDefaults()
    {
        MaxX = GetMaxX();
        MaxY = GetMaxY();

        State = State.Falling;
        GameOverAnimationCoroutine = null;
        RemoveLinesAnimationCoroutine = null;
        IsPaused = true;
        IsLvlReached = false;
        IsAdContinued = false;
        GameMatrix = new GameMatrix<bool>(MaxX, MaxY);
        CurrentObject = new GameMatrix<bool>(0, 0);
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

            if (State == State.GameOver)
            {
                PlayDeathAnimation();
                continue;
            }

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
    protected void PlayDeathAnimation()
    {
        if (GameOverAnimationCoroutine.MoveNext() && GameOverAnimationCoroutine.Current) return;

        IsPaused = true;
        Menu.SetMenu<GameOverMenu>();

        if (BlindRendering)
        {
            BlindRendering = false;
            FieldRenderer.UpdateField();
            BlindRendering = true;
        }
    }
    protected void Rotate()
    {
        var wasWaiting = State == State.WaitingOnGround;

        var rotate = Controls.Rotate;
        var rotatec = Controls.RotateC;

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

        var right = Controls.Right;
        var left = Controls.Left;

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

        if (State == State.Falling && Controls.Down)
        {
            WaitDownTime -= LevelSpeed / 3 + 1;
            Score++;
        }
        else if (State == State.WaitingOnGround && Controls.Down)
            WaitDownTime = 0;
        else if (State == State.HoldingOnTop && (Controls.Left || Controls.Right || Controls.Rotate || Controls.RotateC))
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

                if (State == State.GameOver) return;

                WaitDownTime = 20;
                State = State.HoldingOnTop;
            }
            else
            {
                Lines += LinesToRemove.Length;
                Score += GetScoreForLine(LinesToRemove.Length, LinesToRemove[0]);
                Lvl = GetLvlFor(Lines, StartLvl);

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
        if (CurrentShape == Shapes.O) return;

        var newObject = clockwise ?
            CurrentObject.RotateClockwise() :
            CurrentObject.RotateAntiClockwise();

        int cx = 0, cy = 0;
        bool rotate = false;
        (int x, int y) [] wkd;

        for (int i = 0; i < 5; i++)
        {
            wkd = (CurrentShape == Shapes.I ? TetrominoWallKickDataI : TetrominoWallKickData) [clockwise ? CurrentRotation : (CurrentRotation - 1 + 4) % 4];

            cx = CurrentX + (clockwise ? wkd[i].x : -wkd[i].x);
            cy = CurrentY + (clockwise ? wkd[i].y : -wkd[i].y);

            if (CanBePlaced(cx, cy, GameMatrix, newObject))
            {
                rotate = true;
                break;
            }
        }

        if (!rotate) return;

        CurrentX = cx;
        CurrentY = cy;
        CurrentObject = newObject;
        CurrentRotation += clockwise ? 1 : -1;

        while (CurrentRotation < 0) CurrentRotation += 4;
        if (CurrentRotation > 3) CurrentRotation %= 4;

        FieldRenderer.UpdateFieldFast();
    }
    public static bool CanBePlaced(int cx, int cy, GameMatrix<bool> gameMatrix, GameMatrix<bool> newObject)
    {
        for (int x = 0; x < newObject.MaxX; x++)
            for (int y = 0; y < newObject.MaxY; y++)
                if (x + cx < 0 || x + cx >= gameMatrix.MaxX ||
                    y + cy < 0 || y + cy >= gameMatrix.MaxY ||
                    gameMatrix[x + cx, y + cy] && newObject[x, y])
                    return false;

        return true;
    }
    protected int GetScoreForLine(int linesCount, int minHeight)
    {
        if (linesCount == 1) return (minHeight + 1) * 40 * (Lvl + 1);
        else if (linesCount == 2) return (minHeight + 1) * 100 * (Lvl + 1);
        else if (linesCount == 3) return (minHeight + 1) * 300 * (Lvl + 1);
        else if (linesCount == 4) return (minHeight + 1) * 1200 * (Lvl + 1);

        return 0;
    }
    protected int GetLvlFor(int linesCount, int startLvl)
    {
        if (linesCount >= StartLvl * 10 + 10 || StartLvl >= 15)
        {
            if (StartLvl >= 15) return linesCount / 10 - 5;
            else return linesCount / 10;
        }
        else return startLvl;
    }

    protected int[] GetLinesToRemove()
    {
        var lines = new List<int>(GameMatrix.MaxY);

        for (int y = 0; y < MaxY; y++)
            if (IsLineFull(y))
                lines.Add(y);

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
        State = State.GameOver;
        GameOverAnimationCoroutine = FieldRenderer.GameOverAnimation();
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
        State = State.Falling;
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
    Animation,
    GameOver
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