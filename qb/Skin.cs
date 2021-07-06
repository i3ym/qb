using qb.Gui;

namespace qb
{
    public class Skin
    {
        public readonly TranslatedString Name;
        public readonly qbTexture Atlas;
        public readonly int Size, Index;

        internal Skin(TranslatedString name, qbTexture atlas, int size, int index)
        {
            Name = name;
            Atlas = atlas;
            Size = size;
            Index = index;
        }
    }
}