using System.Linq;
using System.Collections.Immutable;
using osu.Framework.Graphics.Textures;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace qb
{
    public class Skin
    {
        public readonly TranslatedString Name;
        public readonly ImmutableArray<Texture> Textures;
        public readonly int Size, Index;

        public Skin(Image<Rgba32> atlas, int size, int index, TranslatedString name) : this(name, atlas, size, index) { }
        public Skin(TranslatedString name, Image<Rgba32> atlas, int size, int index)
        {
            Name = name;
            Size = size;
            Index = index;

            Textures =
                Enumerable.Range(0, atlas.Width / size)
                .Select(x =>
                {
                    var img = atlas.Clone();
                    img.Mutate(ctx => ctx.Crop(new Rectangle(x * size, index * size, size, size)));

                    var texture = new Texture(img.Width, img.Height, true, osuTK.Graphics.ES30.All.Nearest);
                    texture.SetData(new TextureUpload(img));

                    return texture;
                })
                .ToImmutableArray();
        }
    }
}