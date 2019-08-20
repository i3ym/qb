public class GameMatrix
{
    public int MaxX { get; private set; }
    public int MaxY { get; private set; }
    bool[, ] Matrix;

    public GameMatrix(int maxx, int maxy)
    {
        MaxX = maxx;
        MaxY = maxy;
        Matrix = new bool[MaxX, MaxY];
    }


    public bool this [int x, int y]
    {
        get => (x < 0 || y < 0 || x >= MaxX || y >= MaxY) ? false : Matrix[x, y];
        set
        {
            if (x < 0 || y < 0 || x >= MaxX || y >= MaxY) return;
            Matrix[x, y] = value;
        }
    }

    public void Clear() => Matrix = new bool[MaxX, MaxY];

    //

    public static implicit operator GameMatrix(bool[, ] matrix) => new GameMatrix(matrix.GetLength(0), matrix.GetLength(1)) { Matrix = matrix };
}