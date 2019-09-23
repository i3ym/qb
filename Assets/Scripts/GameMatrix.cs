using System.Collections;
using System.Collections.Generic;

public class GameMatrix<T> : IEnumerable<T>
{
    public readonly int MaxX;
    public readonly int MaxY;
    protected T[, ] Matrix;
    public bool Changed = false;

    public GameMatrix(int maxx, int maxy)
    {
        MaxX = maxx;
        MaxY = maxy;
        Matrix = new T[MaxX, MaxY];
    }


    public T this [int x, int y]
    {
        get => IsInBounds(x, y) ? Matrix[x, y] : default;
        set
        {
            if (IsInBounds(x, y))
            {
                Matrix[x, y] = value;
                Changed = true;
            }
        }
    }

    public T GetWithoutChecks(int x, int y) => Matrix[x, y];
    public bool IsInBounds(int x, int y) => !(x < 0 || y < 0 || x >= MaxX || y >= MaxY);

    public GameMatrix<T> RotateClockwise()
    {
        var newMatrix = new GameMatrix<T>(MaxX, MaxY);

        for (int x = 0; x < MaxX; x++)
            for (int y = 0; y < MaxY; y++)
                newMatrix[y, MaxX - 1 - x] = Matrix[x, y];

        return newMatrix;
    }
    public GameMatrix<T> RotateAntiClockwise()
    {
        var newMatrix = new GameMatrix<T>(MaxX, MaxY);

        for (int x = 0; x < MaxX; x++)
            for (int y = 0; y < MaxY; y++)
                newMatrix[MaxY - 1 - y, x] = Matrix[x, y];

        return newMatrix;
    }

    public void Clear()
    {
        Matrix = new T[MaxX, MaxY];
        Changed = true;
    }
    public GameMatrix<T> Clone()
    {
        var matrix = new GameMatrix<T>(MaxX, MaxY);

        for (int y = 0; y < MaxY; y++)
            for (int x = 0; x < MaxX; x++)
                matrix.Matrix[x, y] = Matrix[x, y];

        return matrix;
    }

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) Matrix.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Matrix.GetEnumerator();

    public static implicit operator GameMatrix<T>(T[, ] matrix) => new GameMatrix<T>(matrix.GetLength(0), matrix.GetLength(1)) { Matrix = matrix };
    public static implicit operator T[, ](GameMatrix<T> matrix) => matrix.Matrix;
}