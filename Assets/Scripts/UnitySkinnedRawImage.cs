using qb.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace qb.Unity
{
    public class UnitySkinnedRawImage : RawImage
    {
        public byte[, ] Tiles
        {
            get => _Tiles;
            set
            {
                _Tiles = value;
                SkinnedTexture = new UnitySkinnedTexture(Tiles);
                rectTransform.sizeDelta = new Vector2(Tiles.GetLength(0), Tiles.GetLength(1));
            }
        }
        byte[, ] _Tiles;

        UnitySkinnedTexture SkinnedTexture;

        public SkinManager SkinManager
        {
            get => _SkinManager;
            set
            {
                if (SkinManager != null) SkinManager.OnSkinChange -= (skin) => SetSkin(skin);

                _SkinManager = value;
                if (SkinManager != null) SkinManager.OnSkinChange += (skin) => SetSkin(skin);
            }
        }
        SkinManager _SkinManager;

        public void SetSkin(Skin skin) => texture = SkinnedTexture.GetTexture(skin, Tiles);
    }
}