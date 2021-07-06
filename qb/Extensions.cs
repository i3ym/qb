using osu.Framework.Localisation;

namespace qb
{
    public static class Extensions
    {
        public static TranslatableString Translatable(this string key, string? fallback = null) => new TranslatableString(key, fallback ?? key);
    }
}