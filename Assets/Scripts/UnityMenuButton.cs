using System;
using qb.Shared;
using UnityEngine;

namespace qb.Unity
{
    public class UnityMenuButton : QBehaviour, IMenuButton
    {
        public bool IsClickable { get; set; }

        public Action OnClick { get; set; } = delegate { };
        public Action OnSelect { get; set; } = delegate { };
        public Action OnDeselect { get; set; } = delegate { };
        public TranslatedString Text
        {
            get => TranslatedText.Text;
            set
            {
                TranslatedText.Text = value;
                transform.sizeDelta = TranslatedText.transform.sizeDelta + new Vector2(2, 0);
            }
        }

        public TranslatedText TranslatedText
        {
            get
            {
                if (_TranslatedText == null)
                {
                    var go = new GameObject(name + "_text");
                    go.transform.parent = transform;

                    _TranslatedText = go.AddComponent<TranslatedText>();
                    _TranslatedText.transform.anchoredPosition = Vector2.zero;

                    Config.OnLanguageChange += (lang) => transform.sizeDelta = TranslatedText.transform.sizeDelta + new Vector2(2, 0);
                }

                return _TranslatedText;
            }
        }

        TranslatedText _TranslatedText;
    }
}