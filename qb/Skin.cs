using osu.Framework.Graphics.Textures;

namespace qb
{
    public class Skin
    {
        public readonly TranslatedString Name;
        public readonly Texture Atlas;
        public readonly int Size, Index;

        internal Skin(TranslatedString name, Texture atlas, int size, int index)
        {
            Name = name;
            Atlas = atlas;
            Size = size;
            Index = index;
        }
    }
}