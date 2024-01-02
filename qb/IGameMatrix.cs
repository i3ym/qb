namespace qb;

public interface IGameMatrix<T> : IEnumerable<T>
{
    bool IsInBounds(int x, int y);

    /// <returns> True if there is at least one cell where both objects is not equals to GameMatrix<T>.Default </returns>
    bool Intersects<T2>(GameMatrix<T2> matrix);

    /// <returns> True if <c>matrix</c> can fit inside <c>this</c> without touch anything that is not GameMatrix<T>.Default </returns>
    bool Fits<T2>(int x, int y, GameMatrix<T2> matrix);
}
