using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osuTK.Input;

namespace qb.Gui
{
    public class PixelButton : CompositeDrawable
    {
        public bool IsMouseOverPressed
        {
            get
            {
                InputManager ??= GetContainingInputManager();

                var state = InputManager.CurrentState;
                if (state.Mouse.IsPressed(MouseButton.Left) && Contains(state.Mouse.Position))
                    return true;

                foreach (var pos in state.Touch.TouchPositions)
                    if (Contains(pos))
                        return true;

                return false;
            }
        }
        public Action MouseDown = delegate { };
        public Action MouseUp = delegate { };
        public Action Action = delegate { };

        public TranslatedString Text { get => PixelText.TranslatedText; set => PixelText.TranslatedText = value; }
        public Colour4 TextColour { get => PixelText.Colour; set => PixelText.Colour = value; }
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

        public readonly PixelText PixelText = new PixelText() { Anchor = Anchor.Centre, Origin = Anchor.Centre };
        InputManager? InputManager;

        public PixelButton() => Height = 2;
        public PixelButton(TranslatedString text) : this() => PixelText.TranslatedText = text;
        public PixelButton(TranslatedString text, Action action) : this(text) => Action = action;

        [BackgroundDependencyLoader]
        void Load()
        {
            if ((RelativeSizeAxes & Axes.X) == 0 && Width == 0)
                AutoSizeAxes = Axes.X;

            AddInternal(PixelText);
        }


        protected override bool OnMouseDown(MouseDownEvent e)
        {
            MouseDown();
            base.OnMouseDown(e);

            return true;
        }
        protected override void OnMouseUp(MouseUpEvent e)
        {
            MouseUp();
            base.OnMouseUp(e);
        }
        protected override bool OnTouchDown(TouchDownEvent e)
        {
            MouseDown();
            return base.OnTouchDown(e);
        }
        protected override void OnTouchUp(TouchUpEvent e)
        {
            MouseUp();
            base.OnTouchUp(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);
            Action();

            return true;
        }
    }
}