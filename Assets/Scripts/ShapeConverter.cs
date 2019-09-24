using static BackgroundMaker;

public static class ShapeConverter
{
    public static Shapes ToShape(Tile tile)
    {
        switch (tile)
        {
            case Tile.CubeG:
                return Shapes.G;
            case Tile.CubeGR:
                return Shapes.GR;
            case Tile.CubeI:
                return Shapes.I;
            case Tile.CubeO:
                return Shapes.O;
            case Tile.CubeT:
                return Shapes.T;
            case Tile.CubeZ:
                return Shapes.Z;
            case Tile.CubeZR:
                return Shapes.ZR;
        };

        return default;
    }

    public static Tile ToTile(Shapes shape)
    {
        switch (shape)
        {
            case Shapes.G:
                return Tile.CubeG;
            case Shapes.GR:
                return Tile.CubeGR;
            case Shapes.I:
                return Tile.CubeI;
            case Shapes.O:
                return Tile.CubeO;
            case Shapes.T:
                return Tile.CubeT;
            case Shapes.Z:
                return Tile.CubeZ;
            case Shapes.ZR:
                return Tile.CubeZR;
        };

        return default;
    }
}