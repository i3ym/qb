using System.Collections;
using System.Collections.Generic;

namespace qb
{
    public struct ReadOnlyGameMatrix<T> : IGameMatrix<T>
    {
        public int Width => Matrix.Width;
        public int Height => Matrix.Height;

        GameMatrix<T> Matrix;

        public ReadOnlyGameMatrix(int width, int height, T defaultValue = default!) => Matrix = new GameMatrix<T>(width, height, defaultValue);

        public T this[int x, int y] => Matrix[x, y];

        public T GetWithoutChecks(int x, int y) => Matrix[x, y];
        public bool Fits<T2>(int x, int y, GameMatrix<T2> matrix) => Matrix.Fits(x, y, matrix);
        public bool Intersects<T2>(GameMatrix<T2> matrix) => Matrix.Intersects(matrix);
        public bool IsInBounds(int x, int y) => Matrix.IsInBounds(x, y);

        public GameMatrix<T> RotateClockwise() => Matrix.RotateClockwise();
        public GameMatrix<T> RotateAntiClockwise() => Matrix.RotateAntiClockwise();

        public IEnumerator<T> GetEnumerator() => Matrix.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => (Matrix as IEnumerable).GetEnumerator();

        public static implicit operator ReadOnlyGameMatrix<T>(T[,] matrix) => new ReadOnlyGameMatrix<T>(matrix.GetLength(0), matrix.GetLength(1)) { Matrix = matrix };
        public static implicit operator ReadOnlyGameMatrix<T>(GameMatrix<T> matrix) => new ReadOnlyGameMatrix<T>(matrix.Width, matrix.Height) { Matrix = matrix };
        public static implicit operator GameMatrix<T>(ReadOnlyGameMatrix<T> matrix) => matrix.Matrix;
    }
}