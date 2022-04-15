using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace qb
{
    public class Skin
    {
        public readonly TranslatedString Name;
        public readonly int Size, Index;
        public ImmutableArray<Texture> Textures { get; private set; }

        public Skin(Image<Rgba32> atlas, int size, int index, TranslatedString name) : this(name, atlas, size, index) { }
        public Skin(TranslatedString name, Image<Rgba32> atlas, int size, int index)
        {
            Name = name;
            Size = size;
            Index = index;

            SetAtlas(atlas);
        }

        public void SetAtlas(Image<Rgba32> atlas)
        {
            Textures =
                Enumerable.Range(0, atlas.Width / Size)
                .Select(x =>
                {
                    var img = atlas.Clone();
                    img.Mutate(ctx => ctx.Crop(new Rectangle(x * Size, Index * Size, Size, Size)));

                    var texture = new Texture(img.Width, img.Height, true, osuTK.Graphics.ES30.All.Nearest);
                    texture.SetData(new TextureUpload(img));

                    return texture;
                })
                .ToImmutableArray();
        }
    }
}