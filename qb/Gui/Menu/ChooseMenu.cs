namespace qb.Gui.Menu
{
    public abstract class ChooseMenu : ControllableMenuScreen
    {
        protected abstract TranslatedString Title { get; }
        MenuButtonContainer ButtonContainer = null!;
        PixelBorderScrollContainer ScrollContainer = null!;

        [BackgroundDependencyLoader]
        void Load()
        {
            AddInternal(new PixelTextButton(Title) { RelativeSizeAxes = Axes.X, Height = 2, Background = Colour4.Black });

            var backb = new PixelTextButton(("Back", "Назад"), Back) { RelativeSizeAxes = Axes.X, Height = 2, Anchor = Anchor.BottomLeft, Origin = Anchor.BottomLeft, Background = Colour4.Black };
            AddInternal(backb);
            AddButton(backb, false);

            ScrollContainer = new PixelBorderScrollContainer()
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
            AddInternal(ScrollContainer);
            AddButtonContainer(ButtonContainer, false);


            SelectionIndex.BindValueChanged(e =>
            {
                if (e.NewValue != 0)
                    ScrollContainer.ScrollIntoView(ButtonContainer[e.NewValue - 1]);
            }, true);

            ScheduleAfterChildren(() =>
            {
                SelectionIndex.Value = ButtonContainer.OfType<IChooseMenuButton>().TakeWhile(x => !x.IsActive).Count() + 1;
                UpdateCursorPosition(0);
            });
        }

        protected void AddButtons(params PixelTextButton[] buttons)
        {
            ButtonContainer.AddRange(buttons);

            foreach (var button in buttons)
                base.AddButton(button, false);
        }
    }
}