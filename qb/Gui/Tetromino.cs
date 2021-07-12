using System.Collections.Immutable;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics.ES30;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace qb.Gui
{
    public class Tetromino : Sprite
    {
        static readonly ImmutableDictionary<Shape, Texture> CachedTextures;

        public Shape Shape { get => ShapeBindable.Value; set => ShapeBindable.Value = value; }
        public new Colour4? Colour { get => ColourBindable.Value; set => ColourBindable.Value = value; }

        public readonly Bindable<Shape> ShapeBindable = new Bindable<Shape>();
        public readonly Bindable<Colour4?> ColourBindable = new Bindable<Colour4?>();

        static Tetromino()
        {
            CachedTextures = Shapes.ShapesList.ToImmutableDictionary(x => x, createTexture);


            Texture createTexture(Shape shape)
            {
                var matrix = shape.Matrix;
                var image = new Image<Rgba32>(matrix.Width, matrix.Height, Color.Transparent);

                var white = (Rgba32) Color.White;
                for (int y = 0; y < matrix.Height; y++)
                    for (int x = 0; x < matrix.Width; x++)
                        if (matrix[x, y])
                            image[x, y] = white;

                var texture = new Texture(image.Width, image.Height, true, All.Nearest);
                texture.SetData(new TextureUpload(image));

                return texture;
            }
        }

        [BackgroundDependencyLoader]
        void Load()
        {
            ShapeBindable.BindValueChanged(_ => RegenerateDrawable(), true);
            ColourBindable.BindValueChanged(e =>
            {
                if (e.NewValue != null)
                    base.Colour = e.NewValue.Value;
            }, true);

            RegenerateDrawable();
        }

        void RegenerateDrawable()
        {
            base.Colour = Colour ?? new Colour4(Shape.R, Shape.G, Shape.B, 1f);
            Texture = CachedTextures[Shape];
        }
    }
}