namespace qb;

public static class Shapes
{
    public static readonly Shape G, GR, T, Z, ZR, I, O;
    public static readonly ImmutableArray<Shape> ShapesList;

    static Shapes()
    {
        G = new Shape(CubeTile.CubeG, 0, .5f, .5f, "001 111 000");
        GR = new Shape(CubeTile.CubeGR, 1, 0, 0, "100 111 000");
        I = new Shape(CubeTile.CubeI, 0, 0, 1, "0000 1111 0000 0000");
        O = new Shape(CubeTile.CubeO, 1, 1, 0, "11 11");
        T = new Shape(CubeTile.CubeT, .5f, 1, .5f, "010 111 000");
        Z = new Shape(CubeTile.CubeZ, 1, .5f, 0, "110 011 000");
        ZR = new Shape(CubeTile.CubeZR, 1, 0, 1, "011 110 000");

        ShapesList = new[] { G, ZR, GR, O, T, Z, I }.ToImmutableArray();
    }

    public static Shape FromTile(CubeTile tile) => ShapesList[(int) tile];
}

public class Shape
{
    public readonly ReadOnlyGameMatrix<TileBool> Matrix;
    public readonly CubeTile Tile;
    public readonly float R, G, B;

    public Shape(CubeTile tile, float r, float g, float b, string matrix) : this(tile, r, g, b, MatrixFromString(tile, matrix)) { }
    public Shape(CubeTile tile, float r, float g, float b, GameMatrix<TileBool> matrix)
    {
        Tile = tile;
        Matrix = matrix;
        (R, G, B) = (r, g, b);
    }

    static GameMatrix<TileBool> MatrixFromString(CubeTile tile, string text)
    {
        var spt = text.Split(' ');
        var output = new GameMatrix<TileBool>(spt[0].Length, spt.Length, CubeTile.None);

        for (int y = 0; y < spt.Length; y++)
            for (int x = 0; x < spt[y].Length; x++)
                output[x, y] = spt[spt[y].Length - 1 - y][x] == '1' ? tile : CubeTile.None;

        return output;
    }
}
