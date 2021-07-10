using System.Collections.Generic;
using System.Collections.Immutable;

namespace qb
{
    public readonly struct TranslatedString
    {
        public bool IsEmpty => Strings?.IsEmpty ?? true;

        readonly ImmutableDictionary<string, string> Strings;

        public TranslatedString(string text) : this(text, text) { }
        public TranslatedString(string eng, string rus) => Strings = new Dictionary<string, string>() { ["en-US"] = eng, ["ru-RU"] = rus, }.ToImmutableDictionary();

        public string this[string lang] => Strings is { IsEmpty: false } && Strings.TryGetValue(lang, out var str) || Strings.TryGetValue("en-US", out str) ? str : string.Empty;

        public static implicit operator TranslatedString(string text) => new TranslatedString(text);
        public static implicit operator TranslatedString((string eng, string rus) text) => new TranslatedString(text.eng, text.rus);
    }
}