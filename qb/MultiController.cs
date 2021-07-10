using System.Collections.Immutable;

namespace qb
{
    public class MultiController : IController
    {
        readonly ImmutableArray<IController> Controllers;

        public MultiController(params IController[] controllers) => Controllers = controllers.ToImmutableArray();

        public ControllerState GetState()
        {
            var state = new ControllerState();

            foreach (var controller in Controllers)
                state = state.Or(controller.GetState());

            return state;
        }
    }
}