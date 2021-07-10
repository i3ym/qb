using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;

namespace qb.Gui
{
    public class ControllerButton : Box
    {
        public Action MouseDown = delegate { };
        public Action MouseUp = delegate { };

        public ControllerButton()
        {

        }
        public ControllerButton(Anchor anchor) : this()
        {
            Anchor = Origin = anchor;
        }


        protected override bool OnMouseDown(MouseDownEvent e)
        {
            MouseDown();
            return base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseUpEvent e)
        {
            MouseUp();
            base.OnMouseUp(e);
        }
    }
}