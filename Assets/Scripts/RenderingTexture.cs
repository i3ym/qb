public class RenderingTexture
{
    public static RenderingTexture Classic = new RenderingTexture("Classic", 0, 0);

    public string Name { get; private set; }
    public int OffsetX { get; private set; }
    public int OffsetY { get; private set; }

    public RenderingTexture(string name, int offsetx, int offsety)
    {
        Name = name;
        offsetx = OffsetX;
        offsety = OffsetY;
    }
}