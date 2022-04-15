namespace qb.Gui
{
    public interface IControllerButton : IHandleable
    {
        QbKey Key { get; }
    }
    public class ControllerButton<T> : PixelButton<T>, IControllerButton where T : Drawable
    {
        [Resolved] KeyboardController KeyboardController { get; set; } = null!;

        public bool IsPressed => IsMouseOverPressed;
        public QbKey Key { get; }
        bool WasPressedLastFrame = false;

        public ControllerButton(QbKey key, T child) : base(child)
        {
            Size = new osuTK.Vector2(1);
            Key = key;
        }

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