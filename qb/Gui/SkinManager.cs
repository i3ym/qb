using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace qb.Gui;

public class SkinManager
{
    [Resolved] Storage Storage { get; set; } = null!;

    public readonly Bindable<Skin> SelectedSkin = new Bindable<Skin>();
    public ImmutableArray<Skin> Skins { get; private set; }
    readonly ImmutableArray<Skin> BaseSkins;

    public SkinManager(Storage storage, ResourceStore<byte[]> store, Bindable<int> selectedSkinBindable)
    {
        Skins = BaseSkins = LoadSkins("Resources/BaseSkins/", store.GetStream).ToImmutableArray();
        ReloadCustomSkins(storage);

        SelectedSkin.BindValueChanged(e => selectedSkinBindable.Value = Skins.IndexOf(e.NewValue));
        selectedSkinBindable.BindValueChanged(e => SelectedSkin.Value = Skins[e.NewValue % Skins.Length], true);
    }

    public void ReloadCustomSkins() => ReloadCustomSkins(Storage);
    void ReloadCustomSkins(Storage storage)
    {
        Skins = BaseSkins.Concat(LoadSkins(storage, "skins")).ToImmutableArray();
        SelectedSkin.TriggerChange();
    }


    IEnumerable<Skin> LoadSkins(Storage storage, string path)
    {
        try { return storage.GetDirectories(path).SelectMany(path => LoadSkins(path, path => storage.GetStream(path, FileAccess.Read, FileMode.Open))); }
        catch { return Enumerable.Empty<Skin>(); }
    }
    IEnumerable<Skin> LoadSkins(string basepath, Func<string, Stream> openfilefunc)
    {
        try
        {
            SkinsData data;
            using (var jsons = openfilefunc(Path.Combine(basepath, "skins.json")))
                data = new JsonSerializer().Deserialize<SkinsData>(new JsonTextReader(new StreamReader(jsons)));

            Image<Rgba32> atlas;
            using (var atlass = openfilefunc(Path.Combine(basepath, data.AtlasFile)))
                atlas = SixLabors.ImageSharp.Image.Load<Rgba32>(atlass);

            return data.Skins.Select((skin) =>
            {
                Image<Rgba32>? background = null;
                try
                {
                    using (var backgrounds = openfilefunc(Path.Combine(basepath, skin.BackgroundFile)))
                        background = SixLabors.ImageSharp.Image.Load<Rgba32>(backgrounds);
                }
                catch { }

                return new Skin(new TranslatedString(skin.NameEn, skin.NameRu), atlas, background, data.Resolution, skin.AtlasIndex);
            });
        }
        catch { return Enumerable.Empty<Skin>(); }
    }


    readonly record struct SkinsData(int Resolution, string AtlasFile, ImmutableArray<SkinData> Skins);
    readonly record struct SkinData(string NameRu, string NameEn, string BackgroundFile, int AtlasIndex);
}
