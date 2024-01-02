namespace qb.Gui;

public partial class FieldGrid : CompositeDrawable
{
    public ReadOnlyGameMatrix<UvSprite> GameObjects => _GameObjects;
    readonly GameMatrix<UvSprite> _GameObjects;


    public FieldGrid(int width, int height)
    {
        Width = width;
        Height = height;

        _GameObjects = new GameMatrix<UvSprite>(width, height);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                var sprite = new UvSprite() { RelativePositionAxes = Axes.Both, Size = new osuTK.Vector2(1), X = x / Width, Y = y / Height, Alpha = 0 };

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
