using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using qb.Gui.Menu;

namespace qb.Gui
{
    [Cached]
    public class PixelScreenStack : Container<MenuScreen>
    {
        public MenuScreen? Root => ScreensPushed?.FirstOrDefault();
        readonly Stack<MenuScreen> ScreensPushed = new Stack<MenuScreen>();

        [BackgroundDependencyLoader]
        void Load() => RelativeSizeAxes = Axes.Both;

        public override void Add(MenuScreen drawable)
        {
            drawable.Hide();
            base.Add(drawable);
        }


        public void Push(MenuScreen screen)
        {
            if (!Contains(screen)) Add(screen);

            screen.Show();
            ScreensPushed.TryPeek(out var last);
            last?.Hide();

            Schedule(() => screen.OnShow(last));
            ScreensPushed.Push(screen);
        }
        public void Pop()
        {
            if (ScreensPushed.TryPop(out var screen))
            {
                screen.Hide();

                if (ScreensPushed.TryPeek(out var newscreen))
                    newscreen.Show();
            }
        }

        public void PopTo(MenuScreen? screen)
        {
            if (screen is null)
            {
                while (ScreensPushed.Count != 0)
                    Pop();

                return;
            }

            if (!ScreensPushed.Contains(screen)) return;

            while (ScreensPushed.Peek() != screen)
                Pop();
        }
    }
}