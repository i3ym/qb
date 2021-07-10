using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace qb.Gui
{
    public class ControllerButton : PixelButton
    {
        readonly QbKey Key;

        public ControllerButton(QbKey key)
        {
            Key = key;
            Size = new(1);
            Background = Colour4.BlueViolet;
        }
        public ControllerButton(QbKey key, Anchor anchor) : this(key)
        {
            Anchor = Origin = anchor;
        }

        [BackgroundDependencyLoader]
        void Load(GameScreen gamesc)
        {
            MouseDown += () => gamesc.Controller.TriggerPressed(Key);
            MouseUp += () => gamesc.Controller.TriggerReleased(Key);
        }
    }
}