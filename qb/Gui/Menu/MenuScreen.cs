using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace qb.Gui.Menu
{
    public class MenuScreen : CompositeDrawable
    {
        [Resolved] PixelScreenStack PixelScreenStack { get; set; } = null!;
        protected readonly Background Background = new Background(Colour4.Black.MultiplyAlpha(.7f));

        [BackgroundDependencyLoader]
        void Load()
        {
            RelativeSizeAxes = Axes.Both;
            AddInternal(Background);
        }

        protected void Push<T>() where T : MenuScreen, new()
        {
            var screen = PixelScreenStack.OfType<T>().FirstOrDefault();

            if (screen is { }) PixelScreenStack.Push(screen);
            else PixelScreenStack.Push(new T());
        }
        protected void Push<T>(T screen) where T : MenuScreen => PixelScreenStack.Push(screen);
        protected void Back() => PixelScreenStack.Pop();
        protected void BackToRoot() => BackTo(PixelScreenStack.Root);
        void BackTo(MenuScreen? screen) => PixelScreenStack.PopTo(screen);

        public virtual void OnShow(MenuScreen? last) { }


        protected override bool Handle(UIEvent e) => true;
    }
}