using System.Collections.Generic;
using osu.Framework.Input.Bindings;

namespace qb
{
    public class KeyboardController : KeyBindingContainer<QbKey>, IController
    {
        public override IEnumerable<IKeyBinding> DefaultKeyBindings { get; } = new IKeyBinding[]
        {
            new KeyBinding(InputKey.W, QbKey.Up),
            new KeyBinding(InputKey.S, QbKey.Down),
            new KeyBinding(InputKey.A, QbKey.Left),
            new KeyBinding(InputKey.D, QbKey.Right),

            new KeyBinding(InputKey.Up, QbKey.Up),
            new KeyBinding(InputKey.Down, QbKey.Down),
            new KeyBinding(InputKey.Left, QbKey.Left),
            new KeyBinding(InputKey.Right, QbKey.Right),

            new KeyBinding(InputKey.E, QbKey.A),
            new KeyBinding(InputKey.Q, QbKey.B),
            new KeyBinding(InputKey.Escape, QbKey.Back),
        };

        public KeyboardController() : base(SimultaneousBindingMode.Unique) { }

        public ControllerState GetState()
        {
            var state = new ControllerState();

            foreach (var button in PressedActions)
                state = state.With(button);

            return state;
        }
    }
}