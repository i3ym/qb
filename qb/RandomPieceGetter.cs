namespace qb;

public class RandomPieceGetter
{
    public int Seed { get; private set; }
    IEnumerator<Shape> Randomizer = null!;

    public RandomPieceGetter(int? seed = null) => Reset(seed);

    public void Reset(int? seed = null) => Randomizer = PieceRandomizer(seed ?? Environment.TickCount);
    public Shape Get()
    {
        Randomizer.MoveNext();
        return Randomizer.Current;
    }

    IEnumerator<Shape> PieceRandomizer(int seed)
    {
        var rnd = new Random(Seed = seed);
        var order = new List<Shape>();
        var pool = Enumerable.Repeat(Shapes.ShapesList, 5).SelectMany(x => x).ToArray();
        var firstPiece = new[] { Shapes.I, Shapes.G, Shapes.GR, Shapes.T }[rnd.Next(4)];
        yield return firstPiece;

        var history = new[] { Shapes.ZR, Shapes.Z, Shapes.ZR, firstPiece };

        int i = 0;
        Shape? piece = null;
        while (true)
        {
            for (int roll = 0; roll < 6; roll++)
            {
                i = rnd.Next(35);
                piece = pool[i];

                if (!history.Any(x => x == piece) || roll == 5)
                    break;

                if (order.Count != 0) pool[i] = order[0];
            }
            if (piece is null) throw new InvalidOperationException();

            if (order.Contains(piece))
                order.Remove(piece);

            order.Add(piece);

            pool[i] = order[0];
            history = history.Skip(1).ToArray();

            if (history.Length < 4) history = history.Append(piece).ToArray();
            else history[3] = piece;

            yield return piece;
        }
    }
}
