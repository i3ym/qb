using System.Collections.ObjectModel;
using System.Linq;

namespace qb
{
    public static class Shapes
    {
        public static readonly int MaxX, MaxY;
        public static readonly Shape G, GR, T, Z, ZR, I, O;
        public static readonly ReadOnlyCollection<Shape> ShapesList;

        static Shapes()
        {
            G = new Shape(CubeTile.CubeG, (0, .5f, .5f), "001\n 111\n 000");
            GR = new Shape(CubeTile.CubeGR, (1, 0, 0), "100\n 111\n 000");
            I = new Shape(CubeTile.CubeI, (0, 0, 1), "0000\n 1111\n 0000\n 0000");
            O = new Shape(CubeTile.CubeO, (1, 1, 0), "11\n 11");
            T = new Shape(CubeTile.CubeT, (.5f, 1, .5f), "010\n 111\n 000");
            Z = new Shape(CubeTile.CubeZ, (1, .5f, 0), "110\n 011\n 000");
            ZR = new Shape(CubeTile.CubeZR, (1, 0, 1), "011\n 110\n 000");

            ShapesList = new ReadOnlyCollection<Shape>(new [] { G, ZR, GR, O, T, Z, I });

            MaxX = ShapesList.Max(x => x.Matrix.MaxX);
            MaxY = ShapesList.Max(x => x.Matrix.MaxY);
        }

        public static Shape FromTile(CubeTile tile) => ShapesList[(int) tile];
    }

    public class Shape
    {
        public readonly ReadOnlyGameMatrix<TileBool> Matrix;
        public readonly CubeTile Tile;
        public readonly(float R, float G, float B) Rgb;

        public Shape(CubeTile tile, (float, float, float) rgb, string matrix) : this(tile, rgb, MatrixFromString(tile, matrix)) { }
        public Shape(CubeTile tile, (float, float, float) rgb, GameMatrix<TileBool> matrix)
        {
            Tile = tile;
            Matrix = matrix;
            Rgb = rgb;
        }

        static GameMatrix<TileBool> MatrixFromString(CubeTile tile, string text)
        {
            var spt = text.Replace(" ", "").Replace(" ", "").Replace("\r", "").Split('\n');
            GameMatrix<TileBool> output = new GameMatrix<TileBool>(spt[0].Length, spt.Length, CubeTile.None);

            for (int y = 0; y < spt.Length; y++)
                for (int x = 0; x < spt[y].Length; x++)
                    output[x, y] = spt[spt[y].Length - 1 - y][x] == '1' ? tile : CubeTile.None;

            return output;
        }
    }
}