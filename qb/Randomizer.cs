namespace qb;

public class Randomizer<T>
{
    readonly Random Random = new Random();
    readonly List<(T item, Range chance)> Items = new List<(T item, Range chance)>();

    public Randomizer<T> Add(T item) => Add(item, 100);
    public Randomizer<T> Add(T item, int chance)
    {
        if (chance <= 0) throw new ArgumentException("Chance can not be less than or equals to zero", nameof(chance));

        var lastc = 0;
        if (Items.Count != 0) lastc = Items[^1].chance.End.Value;
        Items.Add((item, lastc..(chance + lastc)));

        return this;
    }
    public Randomizer<T> AddRange(params T[] items) => AddRange(100, items);
    public Randomizer<T> AddRange(int chance, params T[] items)
    {
        foreach (var item in items)
            Add(item, chance);

        return this;
    }
    public Randomizer<T> AddRange(params (T item, int chance)[] items)
    {
        foreach (var item in items)
            Add(item.item, item.chance);

        return this;
    }

    public T Next()
    {
        var rnd = Random.Next(Items[^1].chance.End.Value);
        return Items.First(x => x.chance.Start.Value <= rnd && x.chance.End.Value > rnd).item;
    }
}
