namespace qb.Gui;

public partial class MenuButtonContainer : MenuButtonContainer<Drawable> { }
public partial class MenuButtonContainer<T> : Container<T> where T : Drawable
{
    public Anchor ChildrenAnchor { get => PixelContent.ChildAxes; set => PixelContent.ChildAxes = value; }

    readonly PixelFlowContainer<T> PixelContent = new PixelFlowContainer<T>() { AutoSizeAxes = Axes.Both, Direction = FillDirection.Vertical, Spacing = new osuTK.Vector2(1) };
    protected override Container<T> Content => PixelContent;

    [BackgroundDependencyLoader]
    void Load()
    {
        InternalChild = Content;
    }
}
