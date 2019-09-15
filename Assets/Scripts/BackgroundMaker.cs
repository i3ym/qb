using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class BackgroundMaker
{
    static Dictionary<int, Texture2D> Textures = new Dictionary<int, Texture2D>();
    static Dictionary<int, Texture2D> BottomTextures = new Dictionary<int, Texture2D>();
    static byte[, ] WallsMatrix;

    static BackgroundMaker()
    {
        Textures.Clear();

        WallsMatrix = new byte[17, 21];

        for (int y = 0; y < 21; y++)
            WallsMatrix[10, y] = (byte) Tile.VerticalWall;

        for (int x = 11; x < 17; x++)
            WallsMatrix[x, 16] = (byte) Tile.HorizontalWall;

        for (int x = 0; x < 17; x++)
            WallsMatrix[x, 0] = (byte) Tile.HorizontalWall;

        WallsMatrix[10, 16] = (byte) Tile.VerticalToRightWall;
        WallsMatrix[10, 0] = (byte) Tile.HorizontalToUpWall;
        WallsMatrix[11, 15] = (byte) Tile.TopLeftCorner;
        WallsMatrix[11, 17] = (byte) Tile.BottomLeftCorner;
        WallsMatrix[11, 1] = (byte) Tile.BottomLeftCorner;
    }

    public static Texture2D GetBottomTexture(int skinIndex)
    {
        if (BottomTextures.ContainsKey(skinIndex)) return BottomTextures[skinIndex];

        var tex = new Texture2D(16 * WallsMatrix.GetLength(0), 16, TextureFormat.ARGB32, 0, true);
        tex.filterMode = FilterMode.Point;

        var transparent = new Color(0, 0, 0, 0);
        var atlas = Config.Instance.Atlas;
        int pos;

        // Background

        pos = (byte) Tile.Background * 16;

        for (int x = 0; x < 17; x++)
            for (int yy = 0; yy < 16; yy++)
                for (int xx = 0; xx < 16; xx++)
                    tex.SetPixel(x * 16 + xx, yy, atlas.GetPixel(pos + xx, atlas.height - 1 - (16 * skinIndex + (15 - yy))));

        // /Background
        // Walls

        pos = (byte) Tile.HorizontalWall * 16;

        for (int x = 0; x < tex.width / 16; x++)
            for (int yy = 0; yy < 16; yy++)
                for (int xx = 0; xx < 16; xx++)
                    tex.SetPixel(x * 16 + xx, yy, atlas.GetPixel(xx + pos, atlas.height - 1 - (16 * skinIndex + (15 - yy))));
        // /Walls

        tex.Apply();
        BottomTextures.Add(skinIndex, tex);

        return tex;
    }

    public static Texture2D GetTexture(int skinIndex)
    {
        if (Textures.ContainsKey(skinIndex)) return Textures[skinIndex];

        var tex = new Texture2D(16 * WallsMatrix.GetLength(0), 16 * WallsMatrix.GetLength(1), TextureFormat.ARGB32, 0, true);
        tex.filterMode = FilterMode.Point;

        var transparent = new Color(0, 0, 0, 0);
        var atlas = Config.Instance.Atlas;
        int pos;

        for (int y = 0; y < tex.height; y++)
            for (int x = 0; x < tex.width; x++)
                tex.SetPixel(x, y, transparent);

        // Background

        {
            pos = (byte) Tile.Background * 16;

            void setbg(int x, int y)
            {
                for (int yy = 0; yy < 16; yy++)
                    for (int xx = 0; xx < 16; xx++)
                        tex.SetPixel(x * 16 + xx, y * 16 + yy, atlas.GetPixel(pos + xx, atlas.height - 1 - (16 * skinIndex + (15 - yy))));
            }

            for (int y = 0; y < 21; y++)
                for (int x = 10; x < 17; x++)
                    setbg(x, y);

            for (int x = 0; x < 17; x++)
                setbg(x, 0);
        }

        // /Background
        // Walls

        for (int y = 0; y < tex.height / 16; y++)
            for (int x = 0; x < tex.width / 16; x++)
            {
                pos = WallsMatrix[x, y] * 16;

                if (pos == 0) continue;

                for (int yy = 0; yy < 16; yy++)
                    for (int xx = 0; xx < 16; xx++)
                        SetCombineColor(tex, x * 16 + xx, y * 16 + yy,
                            atlas.GetPixel(pos + xx, atlas.height - 1 - (16 * skinIndex + (15 - yy))));
            }

        // /Walls
        // Bottom

        pos = (byte) Tile.LongBottomStart * 16;

        for (int yy = 0; yy < 16; yy++)
            for (int xx = 0; xx < 16 * ((byte) Tile.LongBottomEnd - (byte) Tile.LongBottomStart); xx++)
                SetCombineColor(tex, xx, yy,
                    atlas.GetPixel(pos + xx, atlas.height - 1 - (16 * skinIndex + (15 - yy))));

        // /Bottom

        tex.Apply();
        Textures.Add(skinIndex, tex);

        return tex;
    }

    [MethodImpl(256)]
    static void SetCombineColor(Texture2D tex, int x, int y, Color clr)
    {
        if (clr.a >.1f) tex.SetPixel(x, y, clr);
    }

    public enum Tile : byte
    {
        CubeG = 0,
        CubeZR = 1,
        CubeGR = 2,
        CubeO = 3,
        CubeT = 4,
        CubeZ = 5,
        CubeI = 6,
        TopLeftCorner = 7,
        BottomLeftCorner = 8,
        VerticalWall = 9,
        HorizontalWall = 10,
        HorizontalToUpWall = 11,
        VerticalToRightWall = 12,
        Background = 13,
        LongBottomStart = 14,
        LongBottomEnd = 29,
    }
}