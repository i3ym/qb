using System;
using System.Collections;
using System.Collections.Generic;

namespace qb
{
    public class GameMatrix<T> : IEnumerable<T>, IGameMatrix<T>
    {
        public readonly int Width, Height;
        readonly T Default;
        T[,] Matrix;

        public GameMatrix(int width, int height, T defaultValue = default!)
        {
            Width = width;
            Height = height;

            Matrix = new T[Width, Height];

            Default = defaultValue;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    Matrix[x, y] = defaultValue;
        }

        public T this[int x, int y]
        {
            get => IsInBounds(x, y) ? Matrix[x, y] : Default;
            set
            {
                if (IsInBounds(x, y))
                    Matrix[x, y] = value;
            }
        }

        public T GetWithoutChecks(int x, int y) => Matrix[x, y];
        public bool IsInBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        /// <returns> True if there is at least one cell where both objects is not equals to GameMatrix<T>.Default </returns>
        public bool Intersects<T2>(GameMatrix<T2> matrix)
        {
            var maxx = Math.Max(Width, matrix.Width);
            var maxy = Math.Max(Height, matrix.Height);

            for (int y = 0; y < maxy; y++)
                for (int x = 0; x < maxx; x++)
                    if (!EqualityComparer<T>.Default.Equals(this[x, y], Default) &&
                        !EqualityComparer<T2>.Default.Equals(matrix[x, y], matrix.Default))
                        return true;

            return false;
        }

        /// <summary> Returns true if <c>matrix</c> can fit inside <c>this</c> without touch anything that is not GameMatrix<T>.Default </summary>
        public bool Fits<T2>(int x, int y, GameMatrix<T2> matrix)
        {
            for (int yy = 0; yy < matrix.Height; yy++)
                for (int xx = 0; xx < matrix.Width; xx++)
                    if (!EqualityComparer<T2>.Default.Equals(matrix[xx, yy], matrix.Default) &&
                        (!IsInBounds(x + xx, y + yy) || !EqualityComparer<T>.Default.Equals(this[x + xx, y + yy], Default)))
                        return false;

            return true;
        }

        public GameMatrix<T> RotateClockwise() // TODO caching
        {
            var newMatrix = new GameMatrix<T>(Width, Height, Default);

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    newMatrix[y, Width - 1 - x] = Matrix[x, y];

            return newMatrix;
        }
        public GameMatrix<T> RotateAntiClockwise() // TODO caching
        {
            var newMatrix = new GameMatrix<T>(Width, Height, Default);

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    newMatrix[Height - 1 - y, x] = Matrix[x, y];

            return newMatrix;
        }

        public GameMatrix<T> Resize(int newMaxX, int newMaxY)
        {
            var output = new GameMatrix<T>(newMaxX, newMaxY, Default);

            for (int y = 0; y < newMaxY; y++)
                for (int x = 0; x < newMaxX; x++)
                    output[x, y] = this[x, y];

            return output;
        }

        public void Clear()
        {
            Matrix = new T[Width, Height];

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    Matrix[x, y] = Default;
        }
        public GameMatrix<T> Clone()
        {
            var matrix = new GameMatrix<T>(Width, Height);

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    matrix.Matrix[x, y] = Matrix[x, y];

            return matrix;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    yield return Matrix[x, y];
        }
        IEnumerator IEnumerable.GetEnumerator() => Matrix.GetEnumerator();

        public T[,] ToArray() => (T[,]) this;

        public static implicit operator GameMatrix<T>(T[,] matrix) => new GameMatrix<T>(matrix.GetLength(0), matrix.GetLength(1)) { Matrix = matrix };
        public static implicit operator T[,](GameMatrix<T> matrix) => matrix.Matrix;
    }
}