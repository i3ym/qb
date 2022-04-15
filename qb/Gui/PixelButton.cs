namespace qb.Gui
{
    public class PixelButton<T> : PixelButton, IHandleable where T : Drawable
    {
        public readonly T Child;

        public PixelButton(T child) => AddInternal(Child = child);
    }
    public class PixelButton : CompositeDrawable, IHandleable
    {
        public bool IsMouseOverPressed
        {
            get
            {
                InputManager ??= GetContainingInputManager();

                var state = InputManager.CurrentState;
                if (state.Mouse.IsPressed(MouseButton.Left) && Contains(state.Mouse.Position))
                    return true;

                foreach (var source in state.Touch.ActiveSources)
                {
                    var pos = state.Touch.GetTouchPosition(source);
                    if (pos.HasValue && Contains(pos.Value))
                        return true;
                }

                return false;
            }
        }
        public Action MouseDown = delegate { };
        public Action MouseUp = delegate { };
        public Action Action = delegate { };

        public bool HandleEvents { get; set; } = true;
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
        InputManager? InputManager;


        public PixelButton() => Height = 2;

        [BackgroundDependencyLoader]
        void Load()
        {
            if ((RelativeSizeAxes & Axes.X) == 0 && Width == 0)
                AutoSizeAxes = Axes.X;
        }


        protected override bool Handle(UIEvent e) => HandleEvents;
        protected override bool OnMouseDown(MouseDownEvent e)
        {
            MouseDown();
            Action();
            base.OnMouseDown(e);

            return Handle(e);
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
            base.OnTouchDown(e);

            return Handle(e);
        }
        protected override void OnTouchUp(TouchUpEvent e)
        {
            MouseUp();
            base.OnTouchUp(e);
        }
    }
}