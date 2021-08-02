using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;

namespace qb.Gui.Menu
{
    public class MenuScreen : CompositeDrawable, IKeyBindingHandler<QbKey>
    {
        [Resolved] protected PixelScreenStack PixelScreenStack { get; private set; } = null!;
        protected readonly Background Background = new Background(Colour4.Black.MultiplyAlpha(.7f));

        [BackgroundDependencyLoader]
        void Load()
        {
            RelativeSizeAxes = Axes.Both;
            AddInternal(Background);
        }

        protected void Push<T>() where T : MenuScreen, new() => PixelScreenStack.Push<T>();
        protected void Back() => PixelScreenStack.Pop();

        public virtual void OnShow(MenuScreen? last) { }



        // block further event propagation
        protected override bool Handle(UIEvent e) => true;
        public virtual bool OnPressed(QbKey action) => IsAlive && Alpha != 0;
        public virtual void OnReleased(QbKey action) { }
    }
}