using System;
using System.Linq;
using qb.Shared;
using UnityEngine;

namespace qb.Unity
{
    public class UnityGameScreen : QBehaviour, IField
    {
        [SerializeField] Texture2D Atlas16 = null;
        [SerializeField] Texture2D Atlas64 = null;
        [SerializeField] UnityFieldGrid _FieldGrid = null;
        [SerializeField] UnityFieldGrid _NextFieldGrid = null;
        [SerializeField] UnityFieldInfo _FieldInfo = null;

        public UnityFieldGrid FieldGrid => _FieldGrid;
        public UnityFieldGrid NextFieldGrid => _NextFieldGrid;
        public UnityFieldInfo FieldInfo => _FieldInfo;

        public int MaxX => FieldGrid.MaxX;
        public int MaxY => FieldGrid.MaxY;

        public SkinManager SkinManager { get; private set; }

        void Start()
        {
            SkinManager = new SkinManager(Atlas16, Atlas64);

            FieldGrid.SkinManager = SkinManager;
            NextFieldGrid.SkinManager = SkinManager;

            foreach (var tex in SkinnedTexture.GetSkinnedTextures(MaxX, MaxY).OrderByDescending(x => x.index))
            {
                var go = new GameObject(tex.ToString(), typeof(RectTransform), typeof(UnitySkinnedRawImage));
                var image = go.GetComponent<UnitySkinnedRawImage>();

                image.transform.SetParent(transform, false);
                image.Tiles = tex.tiles;
                image.rectTransform.anchoredPosition = new Vector2(tex.x, -tex.y);
                image.rectTransform.anchorMax = image.rectTransform.anchorMin = new Vector2(0, 1);
                image.rectTransform.pivot = new Vector2(1, 0);
                image.rectTransform.eulerAngles = new Vector3(0, 0, 180);
                image.transform.localPosition += new Vector3(0, 0, 2);
                image.SkinManager = SkinManager;

                if (tex.index < 0) image.transform.SetAsLastSibling();
                else if (tex.index > 0) image.transform.SetAsFirstSibling();
            }

            SkinManager.SelectedSkinIndex = SkinManager.SelectedSkinIndex;
        }

        public CubeTile this [int x, int y]
        {
            set => FieldGrid[x, y] = value;
        }

        public void SetNextTile(int x, int y, CubeTile tile) => NextFieldGrid[x, y] = tile;

        public void UpdateLevelText(int value) => FieldInfo.UpdateLevelText(value);
        public void UpdateLinesText(int value) => FieldInfo.UpdateLinesText(value);
        public void UpdateScoreText(int value) => FieldInfo.UpdateScoreText(value);
        public void UpdateTimeText(DateTime value) => FieldInfo.UpdateTimeText(value);

        public void Show() => transform.localScale = Vector2.one;
        public void Hide() => transform.localScale = Vector2.zero;
    }
}