using System;
using System.Collections.Generic;
using System.Linq;

namespace qb.Gui
{
    public static class SkinnedTexture
    {
        public static IEnumerable<(int x, int y, int index, CubeTile[,] tiles)> GetSkinnedTextures(int maxx, int maxy) =>
            Enumerable.Empty<Func<int, int, (int, int, int, CubeTile[,])>>()
                .Append(GetInfoBackground)
                .Append(GetBottomWallBackground)
                .Append(GetBottomWallForeground)
                .Append(GetVerticalWall)
                .Append(GetTopHorizontalWall)
                .Select(func => func(maxx, maxy));

        public static (int x, int y, int index, CubeTile[,] tiles) GetInfoBackground(int maxx, int maxy) =>
            (maxx, 0, 999, new GameMatrix<CubeTile>(7, maxy, CubeTile.Background));
        public static (int x, int y, int index, CubeTile[,] tiles) GetBottomWallBackground(int maxx, int maxy)
        {
            var tiles = new GameMatrix<CubeTile>(maxx + 7, 1, (CubeTile) CubeTile.HorizontalWall);
            tiles[maxx, 0] = CubeTile.HorizontalToUpWall;

            return (0, maxy, -5, tiles);
        }
        public static (int x, int y, int index, CubeTile[,] tiles) GetBottomWallForeground(int maxx, int maxy)
        {
            var tiles = new GameMatrix<CubeTile>(maxx, 1, (CubeTile) CubeTile.None);

            for (int x = 0; x < tiles.Width; x++)
                tiles[x, 0] = (CubeTile) ((byte) CubeTile.LongBottomStart + x);

            return (0, maxy, -6, tiles);
        }
        public static (int x, int y, int index, CubeTile[,] tiles) GetVerticalWall(int maxx, int maxy)
        {
            var tiles = new GameMatrix<CubeTile>(2, maxy, (CubeTile) CubeTile.None);

            for (int y = 0; y < tiles.Height; y++)
                tiles[0, y] = CubeTile.VerticalWall;

            tiles[0, 4] = CubeTile.VerticalToRightWall;

            tiles[1, 3] = CubeTile.BottomLeftCorner;
            tiles[1, 5] = CubeTile.TopLeftCorner;
            tiles[1, maxy - 1] = CubeTile.BottomLeftCorner;

            return (maxx, 0, -9, tiles);
        }
        public static (int x, int y, int index, CubeTile[,] tiles) GetTopHorizontalWall(int maxx, int maxy) =>
            (maxx + 1, 4, -9, new GameMatrix<CubeTile>(6, 1, (CubeTile) CubeTile.HorizontalWall));
    }
}