using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TranslatedText : MonoBehaviour
{
    static List<TranslatedText> Texts = new List<TranslatedText>();

    TextMeshProUGUI Text
    {
        get
        {
            if (_Text == null) _Text = GetComponent<TextMeshProUGUI>();
            return _Text;
        }
    }
    TextMeshProUGUI _Text = null;

    Dictionary<Language, string> Languages = new Dictionary<Language, string>();

    [SerializeField]
    string Russian, English;

    void Awake()
    {
        SetStrings(Russian, English);
    }

    public void SetStrings(string russian, string english)
    {
        if (!Texts.Contains(this)) Texts.Add(this);

        if (string.IsNullOrWhiteSpace(russian) || string.IsNullOrWhiteSpace(english))
        {
            Debug.LogWarning("Не настроен перевод для " + name);
            return;
        }

        Languages.Clear();

        Languages.Add(Language.Russian, russian);
        Languages.Add(Language.English, english);

        UpdateText(Config.Language);
    }

    public void UpdateText(Language language)
    {
        Text.text = Languages[language];
        Text.rectTransform.sizeDelta = new Vector2(Languages[language].Split('\n').Max(x => x.Length), Languages[language].Count(x => x == '\n') + 1);
    }

    public static void SetLanguage(Language language)
    {
        foreach (var text in Texts)
            text.UpdateText(language);
    }
}

public enum Language
{
    Russian,
    English
}