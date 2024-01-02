namespace qb.Gui;

public partial class FloatingTetromino : CompositeDrawable
{
    readonly Random Random = new Random();

    [BackgroundDependencyLoader]
    void Load()
    {
        for (int i = 0; i < 100; i++)
            Spawn();
    }

    void Spawn()
    {
        var tetro = new Tetromino() { RelativePositionAxes = Axes.Both, X = (float) Random.NextDouble(), Y = 1.2f, Shape = Shapes.ShapesList[Random.Next(Shapes.ShapesList.Length)] };
        Schedule(() => tetro.MoveToY(-.2f, Random.NextDouble() * 10000 + 5000).Finally(t => { RemoveInternal(t, true); Spawn(); }));

        AddInternal(tetro);
    }
}
