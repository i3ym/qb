namespace qb;

public enum CubeTile : byte
{
    None = byte.MaxValue,
    CubeG = 0,
    CubeZR = 1,
    CubeGR = 2,
    CubeO = 3,
    CubeT = 4,
    CubeZ = 5,
    CubeI = 6,
    TopLeftCorner = 7,
    BottomLeftCorner = 8,
    VerticalWall = 9,
    HorizontalWall = 10,
    HorizontalToUpWall = 11,
    VerticalToRightWall = 12,
    Background = 13,
    LongBottomStart = 14,
    LongBottomEnd = 29,
}
