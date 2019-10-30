using System;
using System.Collections.Generic;
using qb.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace qb.Unity
{
    public class UnityFieldGrid : QBehaviour, IFieldGrid
    {
        static readonly Color Transparent = new Color(0, 0, 0, 0);
        static readonly Color White = new Color(1, 1, 1, 1);

        [SerializeField] int _MaxX, _MaxY;
        [SerializeField] GameObject FieldObjectPrefab = null;

        public int MaxX => _MaxX;
        public int MaxY => _MaxY;

        public ReadonlyGameMatrix<RawImage> GameObjects => _GameObjects;
        GameMatrix<RawImage> _GameObjects = new GameMatrix<RawImage>(0, 0);

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

        void Start() => CreateFieldObjects();

        void CreateFieldObjects()
        {
            _GameObjects = new GameMatrix<RawImage>(MaxX, MaxY);

            RawImage obj;
            for (int x = 0; x < MaxX; x++)
                for (int y = 0; y < MaxY; y++)
                {
                    obj = Instantiate(FieldObjectPrefab).GetComponent<RawImage>();

                    obj.rectTransform.localPosition = Vector2.zero;
                    obj.rectTransform.anchorMax = obj.rectTransform.anchorMin = obj.rectTransform.pivot = Vector2.zero;
                    obj.rectTransform.anchoredPosition = new Vector2(x, y);
                    obj.rectTransform.sizeDelta = Vector2.one;

                    obj.transform.SetParent(transform, false);

                    _GameObjects[x, y] = obj;
                }
        }
        void SetSkin(Skin skin)
        {
            var atlas = skin.GetAtlas<Texture2D>();

            foreach (var obj in _GameObjects)
            {
                obj.texture = atlas;
                obj.uvRect = new Rect((obj.uvRect.x / obj.uvRect.width * skin.Size) / atlas.width, 1f - ((skin.Index + 1) * skin.Size) / (float) atlas.width,
                    (float) skin.Size / atlas.width, (float) skin.Size / atlas.width);
            }
        }

        public CubeTile this [int x, int y]
        {
            set => SetTile(_GameObjects[x, y], value);
        }

        protected void SetTile(RawImage obj, CubeTile tile)
        {
            if (tile == CubeTile.None) obj.color = Transparent;
            else
            {
                obj.color = White;
                obj.uvRect = new Rect(((byte) tile * SkinManager.SelectedSkin.Size) / (float) SkinManager.SelectedSkin.GetAtlas<Texture2D>().width,
                    obj.uvRect.y, obj.uvRect.height, obj.uvRect.width);
            }
        }
    }
}