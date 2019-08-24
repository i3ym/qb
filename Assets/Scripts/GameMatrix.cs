public class GameMatrix
{
    readonly public int MaxX, MaxY;
    bool[, ] Matrix;

    public GameMatrix(int maxx, int maxy)
    {
        MaxX = maxx;
        MaxY = maxy;
        Matrix = new bool[MaxX, MaxY];
    }


    public bool this [int x, int y]
    {
        get => IsInBounds(x, y) ? Matrix[x, y] : false;
        set
        {
            if (IsInBounds(x, y)) Matrix[x, y] = value;
        }
    }

    public bool GetWithoutChecks(int x, int y) => Matrix[x, y];
    public bool IsInBounds(int x, int y) => !(x < 0 || y < 0 || x >= MaxX || y >= MaxY);

    public void Clear() => Matrix = new bool[MaxX, MaxY];

    //

    public static implicit operator GameMatrix(bool[, ] matrix) => new GameMatrix(matrix.GetLength(0), matrix.GetLength(1)) { Matrix = matrix };
}