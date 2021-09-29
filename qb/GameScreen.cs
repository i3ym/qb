using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using qb.Animations;
using qb.Gui;
using qb.Gui.Menu;

namespace qb
{
    public class GameScreen : CompositeDrawable, IKeyBindingHandler<QbKey>
    {
        [Cached] public readonly FieldController Controller;
        [Resolved] PixelScreenStack ScreenStack { get; set; } = null!;
        [Resolved] SkinManager SkinManager { get; set; } = null!;
        [Resolved] Game Game { get; set; } = null!;

        public readonly int FieldWidth, FieldHeight;
        public readonly FieldGrid Playfield, NextGrid;
        Sprite Bg;

        public GameScreen() : this(10, 20) { }
        public GameScreen(int width, int height)
        {
            FieldWidth = width;
            FieldHeight = height;
            Controller = new FieldController(width, height);

            Playfield = new FieldGrid(width, height);
            NextGrid = new FieldGrid(4, 4) { Anchor = Anchor.TopRight, Origin = Anchor.TopRight, X = -1, Y = -1 };

            Bg = new Sprite() { Width = width, Height = height };
        }

        [BackgroundDependencyLoader]
        void Load(PixelScreenStack menus)
        {
            foreach (var tex in getSkinnedTextures(FieldWidth, FieldHeight))
            {
                var bg = new FieldGrid(tex.tiles.GetLength(0), tex.tiles.GetLength(1)) { X = tex.x, Y = tex.y, Depth = tex.index };
                AddInternal(bg);

                for (int y = 0; y < tex.tiles.GetLength(1); y++)
                    for (int x = 0; x < tex.tiles.GetLength(0); x++)
                        bg[x, tex.tiles.GetLength(1) - 1 - y] = tex.tiles[x, y];
            }

            AddInternal(new Background(Colour4.Black) { Depth = 9999 });
            AddInternal(Bg);
            AddInternal(Controller);
            AddInternal(Playfield);
            AddInternal(NextGrid);
            AddInternal(new GameInfo() { Anchor = Anchor.TopRight, Origin = Anchor.TopRight, X = -7, Y = 6 });


            var gameoveranim = new GameOverFillingAnimation();
            var clearlineanim = new ClearLineAnimationSlideDown();

            Controller.OnGameOver += () => gameoveranim.Animation(this, menus.Push<GameOverMenu>);
            Controller.OnLineClear += lines => clearlineanim.Animation(lines, this);
            Controller.OnFieldUpdate += UpdateField;
            Controller.NextShapeBindable.BindValueChanged(e => SetNextShape(e.NewValue), true);



            static IEnumerable<(int x, int y, int index, CubeTile[,] tiles)> getSkinnedTextures(int maxx, int maxy)
            {
                // info bg
                yield return (maxx, 0, 999, new GameMatrix<CubeTile>(7, maxy, CubeTile.Background));

                // bottom wall bg
                {
                    var tiles = new GameMatrix<CubeTile>(maxx + 7, 1, (CubeTile) CubeTile.HorizontalWall);
                    tiles[maxx, 0] = CubeTile.HorizontalToUpWall;

                    yield return (0, maxy, -5, tiles);
                }

                // bottom wall fg
                {
                    var tiles = new GameMatrix<CubeTile>(maxx, 1, (CubeTile) CubeTile.None);
                    for (int x = 0; x < tiles.Width; x++)
                        tiles[x, 0] = (CubeTile) ((byte) CubeTile.LongBottomStart + x);

                    yield return (0, maxy, -6, tiles);
                }

                // vertical wall
                {
                    var tiles = new GameMatrix<CubeTile>(2, maxy, (CubeTile) CubeTile.None);

                    for (int y = 0; y < tiles.Height; y++)
                        tiles[0, y] = CubeTile.VerticalWall;

                    tiles[0, 4] = CubeTile.VerticalToRightWall;
                    tiles[1, 3] = CubeTile.BottomLeftCorner;
                    tiles[1, 5] = CubeTile.TopLeftCorner;
                    tiles[1, maxy - 1] = CubeTile.BottomLeftCorner;

                    yield return (maxx, 0, -9, tiles);
                }

                // horizontal wall
                yield return (maxx + 1, 4, -9, new GameMatrix<CubeTile>(6, 1, (CubeTile) CubeTile.HorizontalWall));
            }
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

        public bool OnPressed(KeyBindingPressEvent<QbKey> e)
        {
            if (e.Action == QbKey.Back)
            {
                Controller.IsPaused.Value = true;
                ScreenStack.Push<PauseMenu>();

                return true;
            }

            return false;
        }
        public void OnReleased(KeyBindingReleaseEvent<QbKey> e) { }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (Game.IsTest)
            {
                var img1 = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(System.IO.File.ReadAllBytes("bg.png"));
                var texture = new osu.Framework.Graphics.Textures.Texture(img1.Width, img1.Height, true, osuTK.Graphics.ES30.All.Nearest);
                texture.SetData(new osu.Framework.Graphics.Textures.TextureUpload(img1));

                Bg.Texture = texture;


                var img2 = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(System.IO.File.ReadAllBytes("atlas.png"));
                foreach (var skin in SkinManager.Skins)
                    skin.SetAtlas(img2);
                SkinManager.SelectedSkin.TriggerChange();
            }

            return base.OnMouseDown(e);
        }
    }
}