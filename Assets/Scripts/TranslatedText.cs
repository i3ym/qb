using System;
using System.Linq;
using qb.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace qb.Unity
{
    [RequireComponent(typeof(RawImage))]
    public class TranslatedText : QBehaviour, IText
    {
        [SerializeField] string Russian = null, English = null;
        [SerializeField] public bool AutoSize = true;

        public TranslatedString Text
        {
            get => _Text;
            set
            {
                _Text = value;
                UpdateTexture();
            }
        }
        TranslatedString _Text;

        public RawImage RawImage => _RawImage == null ?
            ((_RawImage = GetComponent<RawImage>()) == null ?
                (_RawImage = gameObject.AddComponent<RawImage>()) :
                _RawImage) : _RawImage;
        RawImage _RawImage;

        protected override void AwakeOverride() => Config.OnLanguageChange += (lang) => UpdateTexture();
        protected void Start()
        {
            if (Russian != null && English != null) Text = (English, Russian);
        }

        void UpdateTexture()
        {
            int width = Math.Max(Text[Config.Language].Sum(x => FontCharacters.Get(x).Width) + Text[Config.Language].Length - 1, 1);
            int height = FontCharacters.Get(' ').Height;
            var texture = new Texture2D(width, height);
            texture.filterMode = FilterMode.Point;

            int allx = 0;

            Character character;
            foreach (var letter in Text[Config.Language])
            {
                character = FontCharacters.Get(letter);

                for (int y = 0; y < character.Height; y++)
                    for (int x = 0; x < character.Width; x++)
                        texture.SetPixel(x + allx, texture.height - 1 - y, new Color(1, 1, 1, character[x, y] ? 1 : 0));

                allx += character.Width + 1;

                if (allx <= texture.width)
                    for (int y = 0; y < character.Height; y++)
                        texture.SetPixel(allx - 1, texture.height - 1 - y, new Color(1, 1, 1, 0));
            }

            texture.Apply();
            RawImage.texture = texture;

            if (AutoSize)
                transform.sizeDelta = new Vector2(texture.width / 5f, 1f);

            /* transform.sizeDelta = new Vector2(
                 Math.Max(Text[Config.Language].Split('\n').Max(str => str.Sum(x => FontCharacters.Get(x).Width) + str.Length), 1) / 5f + 2,
                 Text[Config.Language].Count(x => x == '\n') + 1);*/
        }
    }
}