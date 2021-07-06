using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK.Input;
using qb.Animations;
using qb.Mods;

namespace qb
{
    public partial class FieldController : CompositeDrawable
    {
        //0R R0 R2 2R 2L L2 L0 0L
        protected static ImmutableArray<ImmutableArray<(int, int)>> TetrominoWallKickData = new[]
        {
            new[] { (0, 0), (-1, 0), (-1, 1), (0, -2), (-1, -2), }.ToImmutableArray(),
            new[] { (0, 0), (1, 0), (1, -1), (0, 2), (1, 2), }.ToImmutableArray(),
            new[] { (0, 0), (1, 0), (1, 1), (0, -2), (1, -2), }.ToImmutableArray(),
            new[] { (0, 0), (-1, 0), (-1, -1), (0, 2), (-1, 2), }.ToImmutableArray(),
        }.ToImmutableArray();
        protected static ImmutableArray<ImmutableArray<(int, int)>> TetrominoWallKickDataI = new[]
        {
            new[] { (0, 0), (-2, 0), (1, 0), (-2, -1), (1, 2), }.ToImmutableArray(),
            new[] { (0, 0), (-1, 0), (2, 0), (-1, 2), (2, -1), }.ToImmutableArray(),
            new[] { (0, 0), (2, 0), (-1, 0), (2, 1), (-1, -2), }.ToImmutableArray(),
            new[] { (0, 0), (1, 0), (-2, 0), (1, -2), (-2, 1), }.ToImmutableArray(),
        }.ToImmutableArray();


        protected static readonly ImmutableArray<int> Levels = new int[] { 52, 48, 44, 40, 36, 32, 27, 21, 16, 10, 9, 8, 7, 6, 5, 5, 4, 3, 3, 2, 2 }.ToImmutableArray();

        public event EventHandler<CancelEventArgs> OnStartGame = delegate { };
        public event Action OnGameOver = delegate { };
        public event Action<int[]> OnLineClear = delegate { };
        /// <summary> Returns false if event needs to be cancelled </summary>
        public Func<bool, bool> OnPauseStateChange = delegate { return true; };

        public virtual int MaxX => 10;
        public virtual int MaxY => 20;
        public int CurrentX { get; protected set; }
        public int CurrentY { get; protected set; }
        public int CurrentRotation { get; protected set; }
        public Shape CurrentShape { get; protected set; }
        public Shape NextShape { get; protected set; }

        public readonly BindableInt Score = new BindableInt();
        public readonly BindableInt Lines = new BindableInt();
        public readonly BindableInt Level = new BindableInt();
        public new readonly BindableInt Time = new BindableInt();

        public GameMatrix<TileBool> CurrentObject { get; protected set; }
        public GameMatrix<TileBool> Tiles { get; protected set; }

        public int StartLevel
        {
            get => _StartLevel;
            set => _StartLevel = Math.Max(0, Math.Min(30, value));
        }
        public bool IsPaused
        {
            get => _IsPaused;
            set
            {
                if (OnPauseStateChange(value)) _IsPaused = value;

                Schedule(() =>
                {
                    if (IsPaused) Field.Hide();
                    else Field.Show();
                });
            }
        }

        protected RandomPieceGetter RandomPieceGetter;

        protected int WaitDownTime = 0, WaitHorizontalTime = 6, WaitLineClearTime = 0;
        protected State State
        {
            get => _State;
            set
            {
                _State = value;

                switch (value)
                {
                    case State.HoldingOnTop:
                        WaitDownTime = 20;
                        break;

                    case State.Falling:
                        WaitDownTime = LevelSpeed;
                        break;

                    case State.WaitingOnGround:
                        WaitDownTime = LevelSpeed + 20;
                        break;
                }
            }
        }
        protected HorizontalState HorizontalState = HorizontalState.Waiting;
        protected RotateState RotateState = RotateState.Waiting;
        protected int[] LinesToRemove;
        protected int LevelSpeed => Level.Value > Levels.Length ? 1 : Levels[Level.Value];

        public Mod[] Mods;

        protected bool _IsPaused = false;
        protected int _Score, _Lines, _Level, _StartLevel;
        protected TimeSpan _Time;
        protected State _State = State.Falling;

        public FieldController(GameScreen field)
        {
            Field = field;
            IsPaused = true;

            StartUpdating();
        }

        protected void StartUpdating()
        {
            var updateThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000 / 60);
                    Update();
                }
            });
            updateThread.IsBackground = true;
            updateThread.Name = "Update Thread";
            updateThread.Start();
        }

        public new void Update()
        {
            if (IsPaused) return;
            if (State == State.GameOver) return;

            Time.Value++;
            if (State == State.LineClearAnimation)
            {
                WaitLineClearTime--;
                if (WaitLineClearTime <= 0)
                {
                    RemoveLines(LinesToRemove);

                    SpawnBlock();
                    State = State.HoldingOnTop;
                }

                return;
            }

            Rotate();
            MoveHorizontal();
            MoveDown();
        }

        public void SetDefaults()
        {
            CurrentX = MaxX / 2 - 1;
            CurrentY = MaxY - 1;
            CurrentRotation = 0;
            Score.Value = 0;
            Lines.Value = 0;
            Level.Value = StartLevel;
            Time.Value = 0;

            CurrentObject = new GameMatrix<TileBool>(0, 0, CubeTile.None);
            Tiles = new GameMatrix<TileBool>(MaxX, MaxY, CubeTile.None);

            IsPaused = true;
            RandomPieceGetter = new RandomPieceGetter();

            WaitDownTime = 0;
            WaitHorizontalTime = 6;
            State = State.Falling;

            UpdateField();
        }
        public void StartGame()
        {
            var args = new CancelEventArgs(false);
            OnStartGame(this, args);
            if (args.Cancel) return;

            SetDefaults();
            IsPaused = false;

            NextShape = Shapes.ShapesList[new Random().Next(Shapes.ShapesList.Count)];
            SpawnBlock();
        }

        protected void Rotate()
        {
            var wasWaiting = State == State.WaitingOnGround;

            var rotate = A;
            var rotatec = B;

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

            var right = Right;
            var left = Left;

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
        protected void MoveDown()
        {
            WaitDownTime--;

            if (State == State.Falling && Down)
                WaitDownTime -= LevelSpeed / 3 + 1;
            else if (State == State.WaitingOnGround && Down)
                WaitDownTime = 0;
            else if (State == State.HoldingOnTop && (Left || Right || A || B))
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
                    Level.Value = GetLvlFor(Lines.Value, StartLevel);

                    State = State.LineClearAnimation;
                    WaitDownTime = 20;
                    WaitLineClearTime = 1000 / 16;

                    OnLineClear(LinesToRemove);
                }

                return;
            }

            CurrentY--;
            UpdateField();

            WaitDownTime = LevelSpeed;

            if (NeedStop()) State = State.WaitingOnGround;
        }
        protected bool NeedStop()
        {
            for (int y = 0; y < CurrentObject.MaxY; y++)
                for (int x = 0; x < CurrentObject.MaxX; x++)
                {
                    if (x + CurrentX >= Tiles.MaxX ||
                        y + CurrentY - 1 >= Tiles.MaxY ||
                        x + CurrentX < 0) continue;

                    if (CurrentObject[x, y] && (y + CurrentY - 1 < 0 || Tiles[x + CurrentX, y + CurrentY - 1]))
                        return true;
                }

            return false;
        }

        protected void SpawnBlock()
        {
            CurrentRotation = 0;
            CurrentShape = NextShape;
            CurrentObject = CurrentShape.Matrix;

            CurrentX = 4;
            CurrentY = Tiles.MaxY - CurrentObject.MaxY;

            for (int y = CurrentObject.MaxY - 1; y >= 0; y--)
            {
                for (int x = 0; x < CurrentObject.MaxX; x++)
                    if (CurrentObject[x, y])
                        goto end;

                CurrentY++;
            }
            end:;

            NextShape = RandomPieceGetter.Get();
            ShowNextShape();

            for (int y = 0; y < CurrentObject.MaxY; y++)
                for (int x = 0; x < CurrentObject.MaxX; x++)
                    if (CurrentObject[x, y] && Tiles[x + CurrentX, y + CurrentY])
                    {
                        GameOver();
                        return;
                    }

            UpdateField();
        }
        protected void SetBlockToField()
        {
            for (int x = 0; x < CurrentObject.MaxX; x++)
                for (int y = 0; y < CurrentObject.MaxY; y++)
                    if (CurrentObject[x, y])
                        Tiles[CurrentX + x, CurrentY + y] = CurrentShape.Tile;

            UpdateField();
        }
        protected void MoveRight()
        {
            for (int y = 0; y < CurrentObject.MaxY; y++)
                for (int x = 0; x < CurrentObject.MaxX; x++)
                    if (CurrentObject.GetWithoutChecks(x, y) && (CurrentX + x + 1 >= Tiles.MaxX || Tiles[CurrentX + x + 1, CurrentY + y]))
                        return;

            CurrentX++;
            UpdateField();
        }
        protected void MoveLeft()
        {
            for (int y = 0; y < CurrentObject.MaxY; y++)
                for (int x = 0; x < CurrentObject.MaxX; x++)
                    if (CurrentObject.GetWithoutChecks(x, y) && (CurrentX + x - 1 < 0 || Tiles[CurrentX + x - 1, CurrentY + y]))
                        return;

            CurrentX--;
            UpdateField();
        }
        protected void RotateBlock(bool clockwise)
        {
            if (CurrentShape == Shapes.O) return;

            var newObject = clockwise ?
                CurrentObject.RotateClockwise() :
                CurrentObject.RotateAntiClockwise();

            int cx = 0, cy = 0;
            bool rotate = false;
            ImmutableArray<(int x, int y)> wkd;

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

            UpdateField();
        }

        protected static int GetScoreForLine(int linesCount, int minHeight, int level)
        {
            if (linesCount == 1) return (minHeight + 1) * 40 * level;
            else if (linesCount == 2) return (minHeight + 1) * 100 * level;
            else if (linesCount == 3) return (minHeight + 1) * 300 * level;
            else if (linesCount == 4) return (minHeight + 1) * 1200 * level;

            return 0;
        }
        protected static int GetLvlFor(int linesCount, int startLevel) => linesCount < startLevel * 10 + 10 ?
            startLevel :
            linesCount / 10;

        protected int[] GetLinesToRemove()
        {
            var lines = new List<int>(Tiles.MaxY);

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
                        Tiles[x2, y2] = Tiles[x2, y2 + 1];

            for (int x2 = 0; x2 < MaxX; x2++)
                Tiles[x2, MaxY - 1] = CubeTile.None;
        }

        protected bool IsLineFull(int y)
        {
            for (int x = 0; x < MaxX; x++)
                if (!Tiles[x, y])
                    return false;

            return true;
        }
        protected bool IsLineEmpty(int y)
        {
            for (int x = 0; x < MaxX; x++)
                if (Tiles[x, y])
                    return false;

            return true;
        }

        protected void RemoveTopLines(int count)
        {
            for (int y = 0; y < count; y++)
                for (int x = 0; x < Tiles.MaxX; x++)
                    Tiles[x, Tiles.MaxY - 1 - y] = CubeTile.None;

            UpdateField();
        }

        protected void GameOver()
        {
            OnGameOver();
            State = State.GameOver;
        }


        IEnumerable<Key>? PressedKeys;
        protected override bool OnKeyDown(KeyDownEvent e)
        {
            PressedKeys = e.PressedKeys;
            return base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyUpEvent e)
        {
            PressedKeys = e.PressedKeys;
            base.OnKeyUp(e);
        }

        bool Left => Check(Key.A) || Check(Key.Left);
        bool Right => Check(Key.D) || Check(Key.Right);
        bool Up => Check(Key.W) || Check(Key.Up);
        bool Down => Check(Key.S) || Check(Key.Down);
        bool A => Check(Key.E);
        bool B => Check(Key.Q);

        bool Check(Key key) => PressedKeys?.Contains(key) ?? false;
    }

    public enum State : byte
    {
        Falling,
        WaitingOnGround,
        HoldingOnTop,
        LineClearAnimation,
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
}