using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranslatedText : MonoBehaviour
{
    static List<TranslatedText> Texts = new List<TranslatedText>();

    TextMeshProUGUI Text;

    Dictionary<Language, string> Languages = new Dictionary<Language, string>();

    [SerializeField]
    string Russian, English;

    void Awake()
    {
        if (string.IsNullOrWhiteSpace(Russian) || string.IsNullOrWhiteSpace(English))
        {
            Debug.LogWarning("Не настроен перевод для " + name);
            return;
        }

        Languages.Add(Language.Russian, Russian);
        Languages.Add(Language.English, English);

        Text = GetComponent<TextMeshProUGUI>();
        Texts.Add(this);
    }

    public static void SetLanguage(Language language)
    {
        foreach (var text in Texts)
        {
            text.Text.text = text.Languages[language];
            text.Text.rectTransform.sizeDelta = new Vector2(text.Languages[language].Length, text.Text.rectTransform.sizeDelta.y);
        }
    }
}

public enum Language
{
    Russian,
    English
}