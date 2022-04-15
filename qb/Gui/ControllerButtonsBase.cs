namespace qb.Gui
{
    public class ControllerButtonsBase : CompositeDrawable
    {
        [Resolved] osu.Framework.Game Game { get; set; } = null!;
        [Resolved] GameScreen GameScreen { get; set; } = null!;

        [BackgroundDependencyLoader]
        void Load()
        {
            AlwaysPresent = true;
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            if ((invalidation & Invalidation.Layout) != 0)
            {
                Schedule(() =>
                {
                    var gameScreenSize = GameScreen.ScreenSpaceDrawQuad.Size;
                    var gameSize = Game.ScreenSpaceDrawQuad.Size;
                    var diff = gameScreenSize - gameSize;

                    var dx = Math.Abs(diff.X);
                    var dy = Math.Abs(diff.Y);

                    if (dx > dy) Scale = new osuTK.Vector2(Math.Min(dx, (gameSize.X - gameScreenSize.X) / 2));
                    else Scale = new osuTK.Vector2(Math.Min(Math.Min(dy, gameSize.Y), gameSize.X / 2.2f));
                });
            }

            return base.OnInvalidate(invalidation, source);
        }
    }
}