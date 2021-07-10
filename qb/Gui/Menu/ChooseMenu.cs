using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace qb.Gui.Menu
{
    public abstract class ChooseMenu : MenuScreen
    {
        protected abstract TranslatedString Title { get; }
        MenuButtonContainer ButtonContainer = null!;

        [BackgroundDependencyLoader]
        void Load(SkinManager skinmgr)
        {
            var buttons = new PixelBorderScrollContainer()
            {
                RelativeSizeAxes = Axes.X,
                Y = 2,
                Height = 17,
                Child = ButtonContainer = new MenuButtonContainer()
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = .9f,
                    ChildrenAnchor = Anchor.TopLeft,
                },
            };


            AddInternal(new PixelButton(Title) { RelativeSizeAxes = Axes.X, Height = 2, Background = Colour4.Black });
            AddInternal(new PixelButton(("Back", "Назад"), Back) { RelativeSizeAxes = Axes.X, Height = 2, Anchor = Anchor.BottomLeft, Origin = Anchor.BottomLeft, Background = Colour4.Black });
            AddInternal(buttons);
        }

        protected void AddButtons(params PixelButton[] buttons) => AddButtons(buttons.AsEnumerable());
        protected void AddButtons(IEnumerable<PixelButton> buttons) => ButtonContainer.AddRange(buttons);
    }
}