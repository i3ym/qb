using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Screens;

namespace qb.Gui.Menu
{
    public class MenuScreen : Screen
    {
        protected readonly Background Background = new(Colour4.Black.MultiplyAlpha(.5f));

        [BackgroundDependencyLoader]
        void Load() => AddInternal(Background);

        protected override bool Handle(UIEvent e) => true;

        protected void Push<T>() where T : Screen, new() => this.Push(new T());
        protected void Back() => this.GetParentScreen().MakeCurrent();
    }
}