namespace qb.Gui.Menu;

public partial class EditControlsMenu : ControllableMenuScreen
{
    [Resolved] Controller Controller { get; set; } = null!;
    [Resolved] EditOverlay EditOverlay { get; set; } = null!;

    [BackgroundDependencyLoader]
    void Load()
    {
        AddButton(new PixelTextButton(("Controller editor", "Редактор контроллера")) { Focusable = false });
        AddButton(new PixelSliderBar(("Scale", "Размер"), 1, 5) { RelativeSizeAxes = Axes.X, Height = 2, Current = { BindTarget = Controller.ChildrenScale } });
        AddButton(new PixelTextButton(("Reset", "Сбросить"), Controller.ResetConfig));
        AddButton(new PixelTextButton(("Back", "Назад"), () => { Controller.Save(); Back(); }));
    }

    public override void OnShow(MenuScreen? last)
    {
        EditOverlay.SetTarget(Controller);
        base.OnShow(last);
    }
    public override void OnRemove()
    {
        EditOverlay.ClearTarget();
        base.OnRemove();
    }
}
