using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

namespace qb.Gui
{
    public class PixelButton : CompositeDrawable
    {
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
        protected override bool OnClick(ClickEvent e)
        {
            base.OnClick(e);
            Action();

            return true;
        }
    }
}