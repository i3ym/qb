namespace qb.Gui;

public partial class PixelFlowContainer : PixelFlowContainer<Drawable> { }
public partial class PixelFlowContainer<T> : FillFlowContainer<T> where T : Drawable
{
    public Anchor ChildAxes { get => ChildAxesBindable.Value; set => ChildAxesBindable.Value = value; }
    public readonly Bindable<Anchor> ChildAxesBindable = new Bindable<Anchor>(Anchor.TopCentre);

    [BackgroundDependencyLoader]
    void Load()
    {
        Spacing = new osuTK.Vector2(1, 0);
        ChildAxesBindable.BindValueChanged(e =>
        {
            foreach (var child in Children)
                child.Anchor = child.Origin = e.NewValue;
        }, true);
    }
}
