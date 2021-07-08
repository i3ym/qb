using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace qb.Gui.Menu
{
    public class SettingsMenu : MenuScreen
    {
        [BackgroundDependencyLoader]
        void Load(SkinManager skinmgr)
        {
            var skinButtons = new MenuButtonContainer()
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = skinmgr.Skins.Select(s => new MenuButton(s.Name, () => skinmgr.SelectedSkin.Value = s)).ToArray(),
            };

            AddInternal(skinButtons);
            AddInternal(new MenuButton(("Back", "Назад"), Back) { Anchor = Anchor.BottomLeft, Origin = Anchor.BottomLeft, X = 1, Y = -1 });
        }
    }
}