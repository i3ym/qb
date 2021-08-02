using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using qb.Gui.Menu;

namespace qb.Gui
{
    [Cached]
    public class PixelScreenStack : Container<MenuScreen>
    {
        [Resolved] GameHost Host { get; set; } = null!;

        public bool IsScreenVisible => ScreensPushed.TryPeek(out var screen) && screen.Alpha != 0;
        readonly Stack<MenuScreen> ScreensPushed = new Stack<MenuScreen>();

        [BackgroundDependencyLoader]
        void Load() => RelativeSizeAxes = Axes.Both;

        public override void Add(MenuScreen drawable)
        {
            drawable.Hide();
            base.Add(drawable);
        }


        public void Push<T>() where T : MenuScreen, new() => Push(new T());
        void Push(MenuScreen screen)
        {
            Add(screen);

            screen.Show();
            ScreensPushed.TryPeek(out var last);
            last?.Hide();

            Schedule(() => screen.OnShow(last));
            ScreensPushed.Push(screen);
        }
        public void Pop()
        {
            if (!ScreensPushed.TryPop(out var screen)) return;
            Remove(screen);

            if (!ScreensPushed.TryPeek(out var newscreen)) return;
            newscreen.Show();
        }
        public void Set(MenuScreen screen)
        {
            if (ScreensPushed.Contains(screen))
                PopTo(null);

            Push(screen);
        }
        public void PopTo(MenuScreen? screen)
        {
            if (screen is null)
            {
                ScreensPushed.Clear();
                Clear();

                return;
            }

            if (!ScreensPushed.Contains(screen)) return;

            while (ScreensPushed.Peek() != screen)
                Pop();
        }
    }
}