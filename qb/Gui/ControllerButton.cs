using osu.Framework.Allocation;
using osu.Framework.Graphics;
using qb.Input;

namespace qb.Gui
{
    public class ControllerButton : PixelButton
    {
        [Resolved] KeyboardController KeyboardController { get; set; } = null!;

        public bool IsPressed => IsMouseOverPressed;
        public readonly QbKey Key;
        bool WasPressedLastFrame = false;

        public ControllerButton(QbKey key)
        {
            Size = new osuTK.Vector2(1);
            Key = key;
        }
        public ControllerButton(QbKey key, Anchor anchor) : this(key) => Anchor = Origin = anchor;

        protected override void Update()
        {
            if (WasPressedLastFrame)
            {
                if (!IsPressed)
                {
                    KeyboardController.TriggerReleased(Key);
                    WasPressedLastFrame = false;
                }
            }
            else
            {
                if (IsPressed)
                {
                    KeyboardController.TriggerPressed(Key);
                    WasPressedLastFrame = true;
                }
            }


            base.Update();
        }
    }
}