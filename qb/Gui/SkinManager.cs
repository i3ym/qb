using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace qb.Gui
{
    public class SkinManager
    {
        public readonly Bindable<Skin> SelectedSkin = new Bindable<Skin>();
        public readonly ImmutableArray<Skin> Skins;

        public SkinManager(TextureStore store, Bindable<int> selectedSkinBindable)
        {
            using var upload = ((ResourceStore<TextureUpload>) store).Get("Resources/Atlas16");
            using var atlas16 = Image<Rgba32>.LoadPixelData(upload.Data, upload.Width, upload.Height);

            Skins = new[]
            {
                new Skin(atlas16, 16, 0,  ("Classic",               "Классический")),
                new Skin(atlas16, 16, 1,  ("Sadness",               "Печаль")),
                new Skin(atlas16, 16, 2,  ("Forest",                "Лес")),
                new Skin(atlas16, 16, 3,  ("Lines",                 "Линии")),
                new Skin(atlas16, 16, 4,  ("Faces",                 "Лица")),
                new Skin(atlas16, 16, 5,  ("Neon",                  "Неон")),
                new Skin(atlas16, 16, 6,  ("Leaf Fall",             "Листопад")),
                new Skin(atlas16, 16, 7,  ("Hot Dog",               "Хот дог")),
                new Skin(atlas16, 16, 8,  ("Vending Machine",       "Торговый автомат")),
                new Skin(atlas16, 16, 9,  ("Medicine",              "Аптечка")),
                new Skin(atlas16, 16, 10, ("New Year",              "Новый год")),
                new Skin(atlas16, 16, 11, ("Beach",                 "Пляж")),
            }.ToImmutableArray();


            SelectedSkin.BindValueChanged(e => selectedSkinBindable.Value = Skins.IndexOf(e.NewValue));
            selectedSkinBindable.BindValueChanged(e => SelectedSkin.Value = Skins[e.NewValue], true);
        }
    }
}