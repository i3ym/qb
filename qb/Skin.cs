using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace qb
{
    public class Skin
    {
        public readonly TranslatedString Name;
        public readonly Texture? Background;
        public ImmutableArray<Texture> Textures { get; private set; }
        readonly int Resolution, Index;

        public Skin(TranslatedString name, Image<Rgba32> atlas, Image<Rgba32>? background, int resolution, int index)
        {
            Name = name;
            Resolution = resolution;
            Index = index;

            if (background is not null)
            {
                Background = new Texture(background.Width, background.Height, true, osuTK.Graphics.ES30.All.Nearest);
                Background.SetData(new TextureUpload(background));
            }

            SetAtlas(atlas);
        }

        public void SetAtlas(Image<Rgba32> atlas)
        {
            Textures =
                Enumerable.Range(0, atlas.Width / Resolution)
                .Select(x =>
                {
                    var img = atlas.Clone();
                    img.Mutate(ctx => ctx.Crop(new Rectangle(x * Resolution, Index * Resolution, Resolution, Resolution)));

                    var texture = new Texture(img.Width, img.Height, true, osuTK.Graphics.ES30.All.Nearest);
                    texture.SetData(new TextureUpload(img));

                    return texture;
                })
                .ToImmutableArray();
        }
    }
}