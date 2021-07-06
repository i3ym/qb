using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using qb.Animations;
using qb.Gui;

namespace qb
{
    public class GameScreen : CompositeDrawable
    {
        [Resolved] public FieldController Controller { get; private init; } = null!;
        public readonly FieldGrid Playfield, NextGrid;

        readonly int FieldWidth, FieldHeight;

        public GameScreen(int width, int height)
        {
            FieldWidth = width;
            FieldHeight = height;

            Width = width + 7;
            Height = height + 1;

            Playfield = new FieldGrid(width, height);
            NextGrid = new FieldGrid(4, 4) { X = (int) Width - 5, Y = -1 };
        }

        [BackgroundDependencyLoader]
        void Load(SkinManager skinmanager)
        {
            foreach (var tex in SkinnedTexture.GetSkinnedTextures(FieldWidth, FieldHeight))
            {
                var bg = new FieldGrid(tex.tiles.GetLength(0), tex.tiles.GetLength(1)) { X = tex.x, Y = tex.y, Depth = tex.index };
                AddInternal(bg);

                for (int y = 0; y < tex.tiles.GetLength(1); y++)
                    for (int x = 0; x < tex.tiles.GetLength(0); x++)
                        bg[x, tex.tiles.GetLength(1) - 1 - y] = tex.tiles[x, y];
            }


            AddInternal(Playfield);
            AddInternal(NextGrid);
            AddInternal(new GameInfo() { X = NextGrid.X - 2, Y = 5 });


            Controller.OnGameOver += () =>
            {
                new GameOverFillingAnimation().Animation(Scheduler, this);
            };
            Controller.OnLineClear += lines =>
            {
                new ClearLineAnimationSlideDown().Animation(lines, Scheduler, this);
            };
        }


        public CubeTile this[int x, int y] { set => Playfield[x, y] = value; }

        public void SetNextTile(int x, int y, CubeTile tile) => NextGrid[x, y] = tile;
    }
}