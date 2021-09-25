using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace qb.Gui.Menu
{
    public class EditControlsMenu : MenuScreen
    {
        [Resolved] Controller Controller { get; set; } = null!;

        [BackgroundDependencyLoader]
        void Load()
        {
            var buttons = new MenuButtonContainer()
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new PixelTextButton(("move padla", "двигай ало")),
                    new PixelSliderBar(("Scale", "Размер"), 1, 5, e => Controller.ChildrenScale = e.NewValue) { RelativeSizeAxes = Axes.X, Height = 2, Current = { Value = Controller.ChildrenScale } },
                    new PixelTextButton(("Back", "Назад"), Back),
                },
            };

            AddInternal(buttons);
        }

        public override void OnShow(MenuScreen? last)
        {
            Controller.EditMode = true;
            base.OnShow(last);
        }
        public override void OnRemove()
        {
            Controller.EditMode = false;
            base.OnRemove();
        }
    }
}