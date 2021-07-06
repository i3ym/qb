using System.Collections.Immutable;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;

namespace qb.Gui
{
    public class SkinManager
    {
        public readonly Bindable<Skin> SelectedSkin = new Bindable<Skin>();
        public readonly BindableInt SelectedSkinIndex = new BindableInt() { MinValue = 0 };
        public ImmutableArray<Skin> Skins;

        public SkinManager(qbTexture atlas)
        {
            int skinidx = 0;
            Skin newSkin(TranslatedString name) => new Skin(name, atlas, 16, skinidx++);

            Skins = new[]
            {
                newSkin(("Classic", "Классический")),
                newSkin(("Sadness", "Печаль")),
                newSkin(("Forest", "Лес")),
                newSkin(("Lines", "Линии")),
                newSkin(("Faces", "Лица")),
                newSkin(("Neon", "Неон")),
                newSkin(("Leaf Fall", "Листопад")),
                newSkin(("Hot Dog", "Хот дог")),
                newSkin(("Vending Machine", "Торговый автомат")),
                newSkin(("New Year", "Новый год")),
            }.ToImmutableArray();

            SelectedSkinIndex.MaxValue = Skins.Length;

            SelectedSkin.BindValueChanged(e => SelectedSkinIndex.Value = Skins.IndexOf(e.NewValue), true);
            SelectedSkinIndex.BindValueChanged(e => SelectedSkin.Value = Skins[e.NewValue], true);
            SelectedSkinIndex.BindTo(Config.SelectedSkin);
        }
    }
}