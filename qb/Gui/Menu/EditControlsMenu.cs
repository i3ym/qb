namespace qb.Gui.Menu
{
    public partial class EditControlsMenu : ControllableMenuScreen
    {
        [Resolved] Controller Controller { get; set; } = null!;

        [BackgroundDependencyLoader]
        void Load()
        {
            AddButton(new PixelTextButton(("move padla", "двигай ало")) { Focusable = false });
            AddButton(new PixelSliderBar(("Scale", "Размер"), 1, 5, e => Controller.ChildrenScale = e.NewValue) { RelativeSizeAxes = Axes.X, Height = 2, Current = { Value = Controller.ChildrenScale } });
            AddButton(new PixelTextButton(("Back", "Назад"), Back));
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