namespace qb.Gui;

public interface IControllerButton : IHandleable
{
    QbKey Key { get; }
}
public partial class ControllerButton<T> : PixelButton<T>, IControllerButton where T : Drawable
{
    [Resolved] KeyboardController KeyboardController { get; set; } = null!;
    public QbKey Key { get; }

    public ControllerButton(QbKey key, T child) : base(child)
    {
        Size = new osuTK.Vector2(1);
        Key = key;

        MouseDown += () => KeyboardController.TriggerPressed(Key);
        MouseUp += () => KeyboardController.TriggerReleased(Key);
    }
}
