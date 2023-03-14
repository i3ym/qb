using osu.Framework.Graphics.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace qb
{
    public class Skin
    {
        public readonly TranslatedString Name;
        public Texture? Background { get; private set; }
        public ImmutableArray<Texture> Textures { get; private set; }
        readonly int Resolution, Index;

        Image<Rgba32> AtlasImage;
        Image<Rgba32>? BackgroundImage;

        public Skin(TranslatedString name, Image<Rgba32> atlas, Image<Rgba32>? background, int resolution, int index)
        {
            Name = name;
            Resolution = resolution;
            Index = index;

            BackgroundImage = background;
            AtlasImage = atlas;

        }

        public void Load(IRenderer renderer)
        {
            if (BackgroundImage is not null)
            {
                Background = renderer.CreateTexture(BackgroundImage.Width, BackgroundImage.Height, true, TextureFilteringMode.Nearest);
                Background.SetData(new TextureUpload(BackgroundImage));

                BackgroundImage = null;
            }

            if (Textures.IsDefaultOrEmpty)
            {
                Textures =
                    Enumerable.Range(0, AtlasImage.Width / Resolution)
                    .Select(x =>
                    {
                        var img = AtlasImage.Clone();
                        img.Mutate(ctx => ctx.Crop(new Rectangle(x * Resolution, Index * Resolution, Resolution, Resolution)));

                        var texture = renderer.CreateTexture(img.Width, img.Height, true, TextureFilteringMode.Nearest);
                        texture.SetData(new TextureUpload(img));

                        return texture;
                    })
                    .ToImmutableArray();
            }
        }
    }
}