using System.Diagnostics;
using osu.Framework.Timing;

namespace qb;

public partial class FieldController : Drawable
{
    public const int MaxFps = 60;
    public const double FrameInterval = 1000d / MaxFps;

    //0R R0 R2 2R 2L L2 L0 0L
    static readonly IReadOnlyList<IReadOnlyList<(int, int)>> TetrominoWallKickData = [
        [(0, 0), (-1, 0), (-1, 1), (0, -2), (-1, -2)],
        [(0, 0), (1, 0), (1, -1), (0, 2), (1, 2)],
        [(0, 0), (1, 0), (1, 1), (0, -2), (1, -2)],
        [(0, 0), (-1, 0), (-1, -1), (0, 2), (-1, 2)],
    ];
    static readonly IReadOnlyList<IReadOnlyList<(int, int)>> TetrominoWallKickDataI = [
        [(0, 0), (-2, 0), (1, 0), (-2, -1), (1, 2)],
        [(0, 0), (-1, 0), (2, 0), (-1, 2), (2, -1)],
        [(0, 0), (2, 0), (-1, 0), (2, 1), (-1, -2)],
        [(0, 0), (1, 0), (-2, 0), (1, -2), (-2, 1)],
    ];
    static readonly IReadOnlyList<int> Levels = [52, 48, 44, 40, 36, 32, 27, 21, 16, 10, 9, 8, 7, 6, 5, 5, 4, 3, 3, 2, 2];

    static readonly ReadOnlyGameMatrix<TileBool> EmptyMatrix = new ReadOnlyGameMatrix<TileBool>(0, 0, CubeTile.None);


    public event Action BeforeUpdate = delegate { };
    public event Action OnStartGame = delegate { };
    public event Action OnGameOver = delegate { };
    public event Action<int[]> OnLineClear = delegate { };
    public event Action OnFieldUpdate = delegate { };
    public event Action OnClearLineAnimationStop = delegate { };

    public int Seed => RandomPieceGetter.Seed;
    int LevelSpeed => Level.Value >= Levels.Count ? 1 : Levels[Level.Value];

    public int CurrentX { get; private set; }
    public int CurrentY { get; private set; }
    public int CurrentRotation { get; private set; }
    public readonly List<Mod> Mods = [];

    public readonly int MaxX, MaxY;
    public readonly Bindable<Shape> CurrentShapeBindable = new Bindable<Shape>();
    public readonly Bindable<Shape> NextShapeBindable = new Bindable<Shape>();
    public readonly BindableBool IsPaused = new BindableBool(true);
    public readonly BindableInt Score = new BindableInt(0);
    public readonly BindableInt Lines = new BindableInt(0);
    public readonly BindableInt Level = new BindableInt(0);
    public readonly BindableInt Frame = new BindableInt(0);
    public readonly BindableInt StartLevel = new BindableInt(0) { MinValue = 0, MaxValue = 30 };

    public ReadOnlyGameMatrix<TileBool> CurrentObject { get; private set; } = EmptyMatrix;
    public readonly GameMatrix<TileBool> Tiles;
    readonly RandomPieceGetter RandomPieceGetter = new RandomPieceGetter();

    public bool RestartOnDeath = false;
    public ReplayRecorder? Recorder;
    public IController? Controller;
    ControllerState ControllerState;


    Shape CurrentShape { get => CurrentShapeBindable.Value; set => CurrentShapeBindable.Value = value; }
    Shape NextShape { get => NextShapeBindable.Value; set => NextShapeBindable.Value = value; }

    int WaitDownTime = 0, WaitHorizontalTime = 6, WaitLineClearTime = 0;
    State State
    {
        get => _State;
        set
        {
            _State = value;

            WaitDownTime = value switch
            {
                State.HoldingOnTop => 20,
                State.Falling => LevelSpeed,
                State.WaitingOnGround => LevelSpeed + 20,
                _ => 20,
            };
        }
    }
    HorizontalState HorizontalState = HorizontalState.Waiting;
    RotateState RotateState = RotateState.Waiting;
    int[] LinesToRemove = Array.Empty<int>();

    State _State;

    public FieldController(int maxx, int maxy)
    {
        MaxX = maxx;
        MaxY = maxy;
        Tiles = new GameMatrix<TileBool>(MaxX, MaxY, CubeTile.None);
    }

    [BackgroundDependencyLoader]
    void Load()
    {
        new Thread(() =>
        {
            var sw = new Stopwatch();
            while (true)
            {
                sw.Restart();
                Schedule(UpdateField);

                Thread.Sleep((int) (1000d / MaxFps - sw.ElapsedMilliseconds));
            }
        })
        { IsBackground = true }.Start();

        StartGame();
        IsPaused.Value = true;
    }

    void UpdateField()
    {
        BeforeUpdate();

        if (IsPaused.Value) return;
        if (State == State.GameOver) return;

        Frame.Value++;
        if (State == State.LineClearAnimation)
        {
            WaitLineClearTime--;
            if (WaitLineClearTime <= 0)
            {
                RemoveLines(LinesToRemove);

                SpawnBlock();
                State = State.HoldingOnTop;
                OnClearLineAnimationStop();
            }

            return;
        }

        ControllerState = Controller?.GetState() ?? default;
        Recorder?.Record(ControllerState);
        Rotate();
        MoveHorizontal();
        MoveDown();
    }

    public void Reset(int? seed = null)
    {
        CurrentX = MaxX / 2 - 1;
        CurrentY = MaxY - 1;
        CurrentRotation = 0;
        Score.SetDefault();
        Lines.SetDefault();
        Frame.SetDefault();
        Level.Value = StartLevel.Value;

        CurrentObject = EmptyMatrix;
        Tiles.Clear();

        IsPaused.Value = true;
        RandomPieceGetter.Reset(seed);

        WaitDownTime = 0;
        WaitHorizontalTime = 6;
        State = State.Falling;

        OnFieldUpdate();
    }
    public void StartGame(bool reset = true, int? seed = null)
    {
        OnStartGame();
        if (reset) Reset(seed);
        IsPaused.Value = false;

        NextShape = RandomPieceGetter.Get();
        SpawnBlock();
    }
    public void StartGame(IController controller, Storage storage)
    {
        Controller = controller;
        RestartOnDeath = false;

        StartGame();
        Recorder = new ReplayRecorder(Seed);
        OnGameOver += () =>
        {
            if (Recorder is null) return;

            try
            {
                using var stream = storage.GetStream(ReplayController.ReplayPath, FileAccess.Write, FileMode.Create);
                stream.Write(Recorder.AsReplay().Serialize(0));
            }
            catch { }

            Recorder = null;
        };
    }

    void Rotate()
    {
        var wasWaiting = State == State.WaitingOnGround;

        var rotate = ControllerState.A;
        var rotatec = ControllerState.B;

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
    void MoveHorizontal()
    {
        if (WaitHorizontalTime > 0)
            WaitHorizontalTime--;

        if (WaitHorizontalTime > 0) return;

        var wasWaiting = State == State.WaitingOnGround;

        var right = ControllerState.Right;
        var left = ControllerState.Left;

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

        if (NeedStop())
        {
            if (State == State.Falling) State = State.WaitingOnGround;
        }
        else if (wasWaiting) State = State.Falling;
    }
    void MoveDown()
    {
        WaitDownTime--;

        if (State == State.Falling && ControllerState.Down)
            WaitDownTime -= LevelSpeed / 3 + 1;
        else if (State == State.WaitingOnGround && ControllerState.Down)
            WaitDownTime = 0;
        else if (State == State.HoldingOnTop && (ControllerState.Left || ControllerState.Right || ControllerState.A || ControllerState.B))
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
                if (State != State.GameOver) State = State.HoldingOnTop;
            }
            else
            {
                Lines.Value += LinesToRemove.Length;
                Score.Value += GetScoreForLine(LinesToRemove.Length, LinesToRemove[0], Level.Value);
                Level.Value = GetLvlFor(Lines.Value, StartLevel.Value);

                State = State.LineClearAnimation;
                WaitDownTime = 20;
                WaitLineClearTime = 1000 / 16;

                OnLineClear(LinesToRemove);
            }

            return;
        }

        CurrentY--;
        OnFieldUpdate();

        WaitDownTime = LevelSpeed;

        if (NeedStop()) State = State.WaitingOnGround;
    }
    bool NeedStop()
    {
        for (int y = 0; y < CurrentObject.Height; y++)
            for (int x = 0; x < CurrentObject.Width; x++)
            {
                if (x + CurrentX >= Tiles.Width ||
                    y + CurrentY - 1 >= Tiles.Height ||
                    x + CurrentX < 0) continue;

                if (CurrentObject[x, y] && (y + CurrentY - 1 < 0 || Tiles[x + CurrentX, y + CurrentY - 1]))
                    return true;
            }

        return false;
    }

    void SpawnBlock()
    {
        CurrentRotation = 0;
        CurrentShape = NextShape;
        CurrentObject = CurrentShape.Matrix;

        CurrentX = 4;
        CurrentY = Tiles.Height - CurrentObject.Height;

        for (int y = CurrentObject.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < CurrentObject.Width; x++)
                if (CurrentObject[x, y])
                    goto end;

            CurrentY++;
        }
        end:;

        NextShape = RandomPieceGetter.Get();

        for (int y = 0; y < CurrentObject.Height; y++)
            for (int x = 0; x < CurrentObject.Width; x++)
                if (CurrentObject[x, y] && Tiles[x + CurrentX, y + CurrentY])
                {
                    GameOver();
                    return;
                }

        OnFieldUpdate();
    }
    void SetBlockToField()
    {
        for (int x = 0; x < CurrentObject.Width; x++)
            for (int y = 0; y < CurrentObject.Height; y++)
                if (CurrentObject[x, y])
                    Tiles[CurrentX + x, CurrentY + y] = CurrentShape.Tile;

        OnFieldUpdate();
    }
    void MoveRight()
    {
        for (int y = 0; y < CurrentObject.Height; y++)
            for (int x = 0; x < CurrentObject.Width; x++)
                if (CurrentObject.GetWithoutChecks(x, y) && (CurrentX + x + 1 >= Tiles.Width || Tiles[CurrentX + x + 1, CurrentY + y]))
                    return;

        CurrentX++;
        OnFieldUpdate();
    }
    void MoveLeft()
    {
        for (int y = 0; y < CurrentObject.Height; y++)
            for (int x = 0; x < CurrentObject.Width; x++)
                if (CurrentObject.GetWithoutChecks(x, y) && (CurrentX + x - 1 < 0 || Tiles[CurrentX + x - 1, CurrentY + y]))
                    return;

        CurrentX--;
        OnFieldUpdate();
    }
    void RotateBlock(bool clockwise)
    {
        if (CurrentShape == Shapes.O) return;

        var newObject = clockwise ?
            CurrentObject.RotateClockwise() :
            CurrentObject.RotateAntiClockwise();

        int cx = 0, cy = 0;
        bool rotate = false;
        IReadOnlyList<(int x, int y)> wkd;

        for (int i = 0; i < 5; i++)
        {
            wkd = (CurrentShape == Shapes.I ? TetrominoWallKickDataI : TetrominoWallKickData)[clockwise ? CurrentRotation : (CurrentRotation - 1 + 4) % 4];

            cx = CurrentX + (clockwise ? wkd[i].x : -wkd[i].x);
            cy = CurrentY + (clockwise ? wkd[i].y : -wkd[i].y);

            if (Tiles.Fits(cx, cy, newObject))
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

        OnFieldUpdate();
    }

    static int GetScoreForLine(int linesCount, int minHeight, int level)
    {
        if (linesCount == 1) return (minHeight + 1) * 40 * (level + 1);
        else if (linesCount == 2) return (minHeight + 1) * 100 * (level + 1);
        else if (linesCount == 3) return (minHeight + 1) * 300 * (level + 1);
        else if (linesCount == 4) return (minHeight + 1) * 1200 * (level + 1);

        return 0;
    }
    static int GetLvlFor(int linesCount, int startLevel) => linesCount < startLevel * 10 + 10 ? startLevel : linesCount / 10;

    int[] GetLinesToRemove()
    {
        var lines = new List<int>(Tiles.Height);

        for (int y = 0; y < MaxY; y++)
            if (IsLineFull(y))
                lines.Add(y);

        return lines.ToArray();
    }
    void RemoveLines(int[] lines)
    {
        for (int i = lines.Length - 1; i > -1; i--)
            for (int y2 = lines[i]; y2 < MaxY - 1; y2++)
                for (int x2 = 0; x2 < MaxX; x2++)
                    Tiles[x2, y2] = Tiles[x2, y2 + 1];

        for (int x2 = 0; x2 < MaxX; x2++)
            Tiles[x2, MaxY - 1] = CubeTile.None;
    }

    bool IsLineFull(int y)
    {
        for (int x = 0; x < MaxX; x++)
            if (!Tiles[x, y])
                return false;

        return true;
    }
    bool IsLineEmpty(int y)
    {
        for (int x = 0; x < MaxX; x++)
            if (Tiles[x, y])
                return false;

        return true;
    }

    void RemoveTopLines(int count)
    {
        for (int y = 0; y < count; y++)
            for (int x = 0; x < Tiles.Width; x++)
                Tiles[x, Tiles.Height - 1 - y] = CubeTile.None;

        OnFieldUpdate();
    }

    void GameOver()
    {
        if (RestartOnDeath)
        {
            Reset();
            SpawnBlock();
            IsPaused.Value = false;

            return;
        }

        OnGameOver();
        State = State.GameOver;
    }
}
public enum State { Falling, WaitingOnGround, HoldingOnTop, LineClearAnimation, GameOver, }
public enum HorizontalState { Waiting, Moving, }
public enum RotateState { Waiting, Rotated, }
