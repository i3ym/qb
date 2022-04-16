namespace qb.Gui.Menu
{
    public abstract class ChooseMenu : ControllableMenuScreen
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


            AddInternal(new PixelTextButton(Title) { RelativeSizeAxes = Axes.X, Height = 2, Background = Colour4.Black });

            AddInternal(buttons);
            AddButtonContainer(ButtonContainer, false);

            var backb = new PixelTextButton(("Back", "Назад"), Back) { RelativeSizeAxes = Axes.X, Height = 2, Anchor = Anchor.BottomLeft, Origin = Anchor.BottomLeft, Background = Colour4.Black };
            AddInternal(backb);
            AddButton(backb, false);

            ScheduleAfterChildren(() => SelectionIndex.Value = ButtonContainer.OfType<IChooseMenuButton>().TakeWhile(x => !x.IsActive).Count() + 1);
        }

        protected void AddButtons(params PixelTextButton[] buttons) => AddButtons(buttons.AsEnumerable());
        protected void AddButtons(IEnumerable<PixelTextButton> buttons)
        {
            ButtonContainer.AddRange(buttons);

            foreach (var button in buttons)
                base.AddButton(button, false);
        }
    }
}