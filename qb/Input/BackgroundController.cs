namespace qb.Input;

public class BackgroundController : IController
{
    public IController Controller = new RandomController();

    public ControllerState GetState() => Controller.GetState();
}
