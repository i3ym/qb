using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace qb.Gui.Menu
{
    public class MenuScreen : Screen
    {
        [Resolved] PixelScreenStack PixelScreenStack { get; set; } = null!;
        protected readonly Background Background = new Background(Colour4.Black.MultiplyAlpha(.7f));

        [BackgroundDependencyLoader]
        void Load() => AddInternal(Background);

        protected void Push<T>() where T : Screen, new() => this.Push(new T());
        protected void Back() => BackTo(this.GetParentScreen());
        protected void BackToRoot() => BackTo(Root);
        void BackTo(IScreen screen)
        {
            (screen as Drawable)?.FadeIn();
            screen.MakeCurrent();
        }

        protected IScreen Root
        {
            get
            {
                var parent = this as IScreen;

                while (true)
                {
                    var p = parent.GetParentScreen();
                    if (p is null) return parent;

                    parent = p;
                }
            }
        }
    }
}