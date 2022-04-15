namespace qb.Gui
{
    public class PixelScrollContainer : ScrollContainer<Drawable>
    {
        [BackgroundDependencyLoader]
        void Load() => ScrollDistance = 2f;

        protected override ScrollbarContainer CreateScrollbar(Direction direction) => new PixelScrollbar(direction);


        class PixelScrollbar : ScrollbarContainer
        {
            public PixelScrollbar(Direction direction) : base(direction) { }

            [BackgroundDependencyLoader]
            void Load() => Add(new Background(Colour4.White));

            public override void ResizeTo(float val, int duration = 0, Easing easing = Easing.None)
            {
                const float thickness = .05f;

                Vector2 size;
                if (ScrollDirection == Direction.Vertical)
                    size = new Vector2(thickness, val);
                else size = new Vector2(val, thickness);

                this.ResizeTo(size, duration, easing);
            }
        }
    }
}