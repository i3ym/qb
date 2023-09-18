namespace qb.Gui;

public interface IEditableDrawable : IDrawable
{
    IEnumerable<Drawable> EditableChildren { get; }
}
public partial class EditOverlay : CompositeDrawable
{
    IEditableDrawable? Drawable;

    public EditOverlay() => RelativeSizeAxes = Axes.Both;

    public void SetTarget(IEditableDrawable drawable)
    {
        ClearTarget();
        Drawable = drawable;

        foreach (var child in Drawable.EditableChildren)
            AddInternal(new EditSelection(child));
    }
    public void ClearTarget()
    {
        if (Drawable is null) return;

        foreach (var child in InternalChildren.OfType<EditSelection>().ToArray())
            RemoveInternal(child, true);
        Drawable = null;
    }

    protected EditSelection Editable(Drawable drawable) => new(drawable);



    protected partial class EditSelection : CompositeDrawable
    {
        readonly Drawable Target;

        public EditSelection(Drawable target) => Target = target;

        [BackgroundDependencyLoader]
        void Load()
        {
            var color = Colour4.Aqua;
            var thickness = 4f;

            AddInternal(new Box() { Anchor = Anchor.TopLeft, Origin = Anchor.TopLeft, RelativeSizeAxes = Axes.X, Height = thickness, Colour = color });
            AddInternal(new Box() { Anchor = Anchor.BottomLeft, Origin = Anchor.BottomLeft, RelativeSizeAxes = Axes.X, Height = thickness, Colour = color });
            AddInternal(new Box() { Anchor = Anchor.TopLeft, Origin = Anchor.TopLeft, RelativeSizeAxes = Axes.Y, Width = thickness, Colour = color });
            AddInternal(new Box() { Anchor = Anchor.TopRight, Origin = Anchor.TopRight, RelativeSizeAxes = Axes.Y, Width = thickness, Colour = color });
        }

        protected override void Update()
        {
            this.MoveTo(Parent.ToLocalSpace(Target.ScreenSpaceDrawQuad.TopLeft));
            this.ResizeTo(Parent.ToLocalSpace(Target.ScreenSpaceDrawQuad.Size));

            base.Update();
        }

        protected override bool Handle(UIEvent e) => true;

        protected override bool OnTouchDown(TouchDownEvent e) => true;
        protected override void OnTouchMove(TouchMoveEvent e)
        {
            Target.MoveToOffset(Parent.ToSpaceOfOtherDrawable(e.Delta, Target.Parent));
            base.OnTouchMove(e);
        }

        protected override bool OnDragStart(DragStartEvent e) => true;
        protected override void OnDrag(DragEvent e)
        {
            Target.MoveToOffset(Parent.ToSpaceOfOtherDrawable(e.Delta, Target.Parent));
            base.OnDrag(e);
        }
    }
}
