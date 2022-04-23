namespace qb.Gui
{
    public class PixelBorderScrollContainer : Container // TODO: make not jest pixelscrorl container but for every thing by input
    {
        readonly PixelScrollContainer ScrollContainer = new PixelScrollContainer() { RelativeSizeAxes = Axes.Both };
        protected override Container<Drawable> Content => ScrollContainer;

        [BackgroundDependencyLoader]
        void Load()
        {
            var container = new Container()
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    Content,
                    new Box() { Origin = Anchor.BottomLeft, RelativeSizeAxes = Axes.X, Height = .1f, Colour = Colour4.Red, Alpha = .5f },
                    new Box() { Anchor = Anchor.BottomLeft, RelativeSizeAxes = Axes.X, Height = .1f, Colour = Colour4.Aqua, Alpha = .5f },
                    new Box() { Origin = Anchor.BottomLeft, RelativeSizeAxes = Axes.X, Height = .2f, Colour = Colour4.Red, Alpha = .5f },
                    new Box() { Anchor = Anchor.BottomLeft, RelativeSizeAxes = Axes.X, Height = .2f, Colour = Colour4.Aqua, Alpha = .5f },
                    // new Tetromino() { Anchor = Anchor.TopLeft, Origin = Anchor.TopRight, Scale = new(.5f), Rotation = -90f, Shape = Shapes.G, Colour = Colour4.Aqua },
                    // new Tetromino() { Anchor = Anchor.BottomRight, Origin = Anchor.TopRight, Scale = new(.5f), Rotation = 90f, Shape = Shapes.G, Colour = Colour4.Red },
                },
            };

            AddInternal(container);
        }

        public void ScrollIntoView(Drawable drawable, bool animated = true) => ScrollContainer.ScrollIntoView(drawable, animated);
    }
}