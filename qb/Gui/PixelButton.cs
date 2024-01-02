namespace qb.Gui;

public partial class PixelButton<T> : PixelButton, IHandleable where T : Drawable
{
    public readonly T Child;

    public PixelButton(T child) => AddInternal(Child = child);
}
public partial class PixelButton : CompositeDrawable, IHandleable, INavigatableButton
{
    public Action MouseDown = delegate { };
    public Action MouseUp = delegate { };
    public Action Action = delegate { };

    public Colour4 Background
    {
        get => BackgroundBox?.Colour ?? Colour4.Transparent;
        set
        {
            if (BackgroundBox is null)
            {
                AddInternal(BackgroundBox = new Background());
                this.ChangeInternalChildDepth(BackgroundBox, 1);
            }

            BackgroundBox.Colour = value;
        }
    }

    Box? BackgroundBox;
    public bool Focusable { get; set; } = true;


    public PixelButton() => Height = 2;

    [BackgroundDependencyLoader]
    void Load()
    {
        if ((RelativeSizeAxes & Axes.X) == 0 && Width == 0)
            AutoSizeAxes = Axes.X;
    }


    protected override bool Handle(UIEvent e) => e is MouseEvent or TouchEvent;
    protected override bool OnMouseDown(MouseDownEvent e)
    {
        MouseDown();
        Action();

        return base.OnMouseDown(e);
    }
    protected override void OnMouseUp(MouseUpEvent e)
    {
        MouseUp();
        base.OnMouseUp(e);
    }
    protected override bool OnTouchDown(TouchDownEvent e)
    {
        MouseDown();
        Action();

        return base.OnTouchDown(e);
    }
    protected override void OnTouchUp(TouchUpEvent e)
    {
        MouseUp();
        base.OnTouchUp(e);
    }

    void INavigatableButton.OnEvent(QbKey action)
    {
        if (action == QbKey.A)
            Action();
    }
}
