using osu.Framework.Graphics.Textures;
using osuTK.Graphics.ES30;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace qb.Gui
{
    public class qbTexture
    {
        public Texture Texture
        {
            get
            {
                if (_Texture is null)
                {
                    _Texture = new Texture(Image.Width, Image.Height, filteringMode: All.Nearest);
                    _Texture.SetData(new TextureUpload(Image.Clone()));
                }

                return _Texture;
            }
        }
        public readonly Image<Rgba32> Image;

        Texture? _Texture;

        public int Width => Image.Width;
        public int Height => Image.Height;

        public qbTexture(int width, int height) : this(new Image<Rgba32>(width, height)) { }
        public qbTexture(Image<Rgba32> image) => Image = image;

        public Rgba32 this[int x, int y] { get => Image[x, y]; set => Image[x, y] = value; }

        public static implicit operator qbTexture(Image<Rgba32> image) => new qbTexture(image);
        public static implicit operator Texture(qbTexture texture) => texture.Texture;
        public static implicit operator Image<Rgba32>(qbTexture texture) => texture.Image;
    }
}