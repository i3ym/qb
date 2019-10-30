using qb.Shared;
using UnityEngine;

namespace qb.Unity
{
    public class UnitySkinnedTexture : SkinnedTexture<Texture2D, Color>
    {
        public UnitySkinnedTexture(byte[, ] tiles) : base(tiles) { }

        protected override Texture2D GetNewTexture(int width, int height) => new Texture2D(width, height) { filterMode = FilterMode.Point };
        protected override Color GetPixel(Texture2D texture, int x, int y) => texture.GetPixel(x, texture.height - 1 - y);
        protected override void SetPixel(Texture2D texture, int x, int y, Color color) => texture.SetPixel(texture.width - 1 - x, y, color);
        protected override Color TransparentColor => new Color(0, 0, 0, 0);

        public override Texture2D GetTexture(Skin skin, byte[, ] tiles)
        {
            var texture = base.GetTexture(skin, tiles);
            texture.Apply();
            return texture;
        }
    }
}