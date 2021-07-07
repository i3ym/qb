using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class FieldGrid : CompositeDrawable
    {
        public ReadOnlyGameMatrix<UvSprite> GameObjects => _GameObjects;
        readonly GameMatrix<UvSprite> _GameObjects;


        public FieldGrid(int width, int height)
        {
            Width = width;
            Height = height;

            _GameObjects = new GameMatrix<UvSprite>(width, height);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    var sprite = new UvSprite() { Width = 1, Height = 1, X = x, Y = y, Alpha = 0 };

                    _GameObjects[x, _GameObjects.Height - 1 - y] = sprite;
                    AddInternal(sprite);
                }
        }

        public CubeTile this[int x, int y] { set => SetTile(GameObjects[x, y], value); }

        void SetTile(UvSprite obj, CubeTile tile)
        {
            Schedule(() =>
            {
                if (tile == CubeTile.None) obj.Alpha = 0;
                else
                {
                    obj.Alpha = 1;
                    obj.Tile.Value = (int) tile;
                }
            });
        }
    }
}