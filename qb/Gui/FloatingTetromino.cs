using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class FloatingTetromino : CompositeDrawable
    {
        readonly Random Random = new();

        [BackgroundDependencyLoader]
        void Load()
        {
            for (int i = 0; i < 100; i++)
                Spawn();
        }

        void Spawn()
        {
            var tetro = new Tetromino() { RelativePositionAxes = Axes.Both, X = (float) Random.NextDouble(), Y = 1.2f, Shape = Shapes.ShapesList[Random.Next(Shapes.ShapesList.Length)] };
            Schedule(() => tetro.MoveToY(-.2f, Random.NextDouble() * 10000 + 5000).Finally(t => { RemoveInternal(t); Spawn(); }));

            AddInternal(tetro);
        }
    }
}