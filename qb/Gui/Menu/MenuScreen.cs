namespace qb.Gui.Menu;

public partial class MenuScreen : CompositeDrawable, IKeyBindingHandler<QbKey>
{
    [Resolved] protected PixelScreenStack PixelScreenStack { get; private set; } = null!;
    protected readonly Background Background = new Background(Colour4.Black.MultiplyAlpha(.7f));

    [BackgroundDependencyLoader]
    void Load()
    {
        RelativeSizeAxes = Axes.Both;
        AddInternal(Background);
    }

    protected void Push<T>() where T : MenuScreen, new() => PixelScreenStack.Push<T>();
    protected void Back() => PixelScreenStack.Pop();

    public virtual void OnShow(MenuScreen? last) { }
    public virtual void OnRemove() { }
    public virtual bool OnPressed(KeyBindingPressEvent<QbKey> e) => IsAlive && Alpha != 0;
    public virtual void OnReleased(KeyBindingReleaseEvent<QbKey> e) { }
}
public partial class ControllableMenuScreen : MenuScreen
{
    Selection SelectionDrawable = null!;
    MenuButtonContainer ButtonContainer = null!;
    public readonly BindableInt SelectionIndex = new(0);
    readonly List<INavigatableButton> Buttons = new();

    [BackgroundDependencyLoader]
    void Load()
    {
        AddInternal(SelectionDrawable = new());

        AddInternal(ButtonContainer = new()
        {
            AutoSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        });


        ScheduleAfterChildren(() => UpdateCursorPosition(0));
    }

    protected override void Update()
    {
        base.Update();
        UpdateCursorPosition();
    }
    protected void UpdateCursorPosition(float duration = 180)
    {
        if (Buttons.Count == 0) return;
        while (SelectionIndex.Value < 0)
            SelectionIndex.Value += Buttons.Count;

        var btn = Buttons[SelectionIndex.Value % Buttons.Count];
        SelectionDrawable
            .MoveTo(btn.ToSpaceOfOtherDrawable(default(Vector2), this), duration, Easing.OutQuad)
            .ResizeTo(((Drawable) btn).LayoutSize, duration, Easing.OutQuad);
    }

    protected void AddButtonContainer<T>(Container<T> container, bool addToContainer = true) where T : Drawable
    {
        if (addToContainer)
        {
            container.Anchor = container.Origin = Anchor.Centre;
            ButtonContainer.Add(container);
        }

        foreach (var btn in container)
            AddButton(btn, false);
    }
    protected void AddButton(Drawable button, bool addToContainer = true)
    {
        if (addToContainer)
        {
            button.Anchor = button.Origin = Anchor.Centre;
            ButtonContainer.Add(button);
        }

        if (button is INavigatableButton navb && navb.Focusable)
            Buttons.Add(navb);
    }

    public override bool OnPressed(KeyBindingPressEvent<QbKey> e)
    {
        if (e.Action == QbKey.Down)
            SelectionIndex.Value = (SelectionIndex.Value + 1) % Buttons.Count;
        else if (e.Action == QbKey.Up)
        {
            var val = SelectionIndex.Value - 1;
            while (val < 0) val += Buttons.Count;

            SelectionIndex.Value = val;
        }
        else (Buttons[SelectionIndex.Value % Buttons.Count] as INavigatableButton)?.OnEvent(e.Action);

        return base.OnPressed(e);
    }

    partial class Selection : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        void Load()
        {
            AddInternal(new Tetromino()
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.BottomRight,
                Shape = Shapes.G,
                Rotation = -90,
                Scale = new(.5f),
                Colour = Colour4.Aqua,
            });
            AddInternal(new Tetromino()
            {
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Shape = Shapes.G,
                Rotation = 90,
                Scale = new(.5f),
                Colour = Colour4.Red,
            });
        }
    }
}
