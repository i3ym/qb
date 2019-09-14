using System.Collections.Generic;
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

        for (int x = 10; x < 17; x++)
            for (int y = 0; y < 21; y++)
                WallsMatrix[x, y] = 15;

        for (int y = 0; y < 21; y++)
            WallsMatrix[10, y] = 11;

        for (int x = 11; x < 17; x++)
            WallsMatrix[x, 16] = 12;

        for (int x = 0; x < 17; x++)
            WallsMatrix[x, 0] = 12;

        WallsMatrix[10, 16] = 14;
        WallsMatrix[10, 0] = 13;
    }

    public static Texture2D GetBottomTexture(int skinIndex)
    {
        if (BottomTextures.ContainsKey(skinIndex)) return BottomTextures[skinIndex];

        var tex = new Texture2D(16 * WallsMatrix.GetLength(0), 16, TextureFormat.ARGB32, 0, true);
        tex.filterMode = FilterMode.Point;

        var atlas = Config.Instance.Atlas;

        for (int x = 0; x < tex.width / 16; x++)
            for (int yy = 0; yy < 16; yy++)
                for (int xx = 0; xx < 16; xx++)
                    tex.SetPixel(x * 16 + xx, yy, atlas.GetPixel(xx + 12 * 16, atlas.height - 1 - (16 * skinIndex + (15 - yy))));

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

        for (int y = 0; y < tex.height / 16; y++)
            for (int x = 0; x < tex.width / 16; x++)
            {
                pos = WallsMatrix[x, y] * 16;

                if (pos == 0)
                    for (int yy = 0; yy < 16; yy++)
                        for (int xx = 0; xx < 16; xx++)
                            tex.SetPixel(x * 16 + xx, y * 16 + yy, transparent);
                else
                    for (int yy = 0; yy < 16; yy++)
                        for (int xx = 0; xx < 16; xx++)
                            tex.SetPixel(x * 16 + xx, y * 16 + yy,
                                atlas.GetPixel(pos + xx, atlas.height - 1 - (16 * skinIndex + (15 - yy))));
            }

        tex.Apply();
        Textures.Add(skinIndex, tex);

        return tex;
    }
}