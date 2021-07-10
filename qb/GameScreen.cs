using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK.Input;
using qb.Animations;
using qb.Gui;
using qb.Gui.Menu;

namespace qb
{
    public class GameScreen : CompositeDrawable
    {
        [Cached] public readonly FieldController Controller;
        [Resolved] PixelScreenStack ScreenStack { get; init; } = null!;

        public readonly int FieldWidth, FieldHeight;
        public readonly FieldGrid Playfield, NextGrid;

        public GameScreen() : this(10, 20) { }
        public GameScreen(int width, int height)
        {
            FieldWidth = width;
            FieldHeight = height;
            Controller = new FieldController(width, height);

            Playfield = new FieldGrid(width, height);
            NextGrid = new FieldGrid(4, 4) { Anchor = Anchor.TopRight, Origin = Anchor.TopRight, X = -1, Y = -1 };
        }

        [BackgroundDependencyLoader]
        void Load(PixelScreenStack menus)
        {
            foreach (var tex in SkinnedTexture.GetSkinnedTextures(FieldWidth, FieldHeight))
            {
                var bg = new FieldGrid(tex.tiles.GetLength(0), tex.tiles.GetLength(1)) { X = tex.x, Y = tex.y, Depth = tex.index };
                AddInternal(bg);

                for (int y = 0; y < tex.tiles.GetLength(1); y++)
                    for (int x = 0; x < tex.tiles.GetLength(0); x++)
                        bg[x, tex.tiles.GetLength(1) - 1 - y] = tex.tiles[x, y];
            }

            AddInternal(new Background(Colour4.Black));
            AddInternal(Controller);
            AddInternal(Playfield);
            AddInternal(NextGrid);
            AddInternal(new GameInfo() { Anchor = Anchor.TopRight, Origin = Anchor.TopRight, X = -7, Y = 6 });


            var gameoveranim = new GameOverFillingAnimation();
            var clearlineanim = new ClearLineAnimationSlideDown();

            Controller.OnGameOver += () => gameoveranim.Animation(Scheduler, this, () => menus.Push(new GameOverMenu()));
            Controller.OnLineClear += lines => clearlineanim.Animation(lines, Scheduler, this);
            Controller.OnFieldUpdate += UpdateField;
            Controller.NextShapeBindable.BindValueChanged(e => SetNextShape(e.NewValue), true);
        }

        void UpdateField()
        {
            var matrix = Controller.Tiles;
            var objectMatrix = Controller.CurrentObject;
            var currentX = Controller.CurrentX;
            var currentY = Controller.CurrentY;

            foreach (var mod in Controller.Mods)
            {
                matrix = mod.OverrideMatrixRender(Controller, matrix);
                objectMatrix = mod.OverrideObjectRender(Controller, objectMatrix);
                mod.OverrideObjectRenderPositions(Controller, ref currentX, ref currentY);
            }

            for (int y = 0; y < Controller.MaxY; y++)
                for (int x = 0; x < Controller.MaxX; x++)
                    Playfield[x, y] = matrix[x, y];

            for (int y = 0; y < objectMatrix.Height; y++)
                for (int x = 0; x < objectMatrix.Width; x++)
                    if (objectMatrix[x, y])
                        Playfield[x + currentX, y + currentY] = objectMatrix[x, y];
        }
        void SetNextShape(Shape shape)
        {
            int addx = 0;
            int addy = 0;

            if (shape == Shapes.O) addx = 1;
            else addy = -1;

            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                    NextGrid[x, y] = shape.Matrix[x - addx, y - addy] ? shape.Tile : CubeTile.None;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape)
            {
                Controller.IsPaused.Value = true;
                ScreenStack.Push(new PauseMenu());
            }

            return base.OnKeyDown(e);
        }
    }
}