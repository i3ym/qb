using osu.Framework.Graphics.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace qb.Gui;

public partial class Tetromino : Sprite
{
    static readonly Dictionary<Shape, Texture> CachedTextures = new();

    public Shape Shape { get => ShapeBindable.Value; set => ShapeBindable.Value = value; }
    public new Colour4? Colour { get => ColourBindable.Value; set => ColourBindable.Value = value; }

    public readonly Bindable<Shape> ShapeBindable = new Bindable<Shape>();
    public readonly Bindable<Colour4?> ColourBindable = new Bindable<Colour4?>();

    [BackgroundDependencyLoader]
    void Load(IRenderer renderer)
    {
        if (CachedTextures.Count == 0)
        {
            foreach (var shape in Shapes.ShapesList)
                CachedTextures[shape] = createTexture(shape);


            Texture createTexture(Shape shape)
            {
                var matrix = shape.Matrix;
                var image = new Image<Rgba32>(matrix.Width, matrix.Height, Color.Transparent);

                var white = (Rgba32) Color.White;
                for (int y = 0; y < matrix.Height; y++)
                    for (int x = 0; x < matrix.Width; x++)
                        if (matrix[x, y])
                            image[x, y] = white;

                var texture = renderer.CreateTexture(image.Width, image.Height, true, TextureFilteringMode.Nearest);
                texture.SetData(new TextureUpload(image));

                return texture;
            }
        }


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
