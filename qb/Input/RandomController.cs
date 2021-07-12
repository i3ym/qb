namespace qb.Input
{
    public class RandomController : IController
    {
        readonly Randomizer<QbKey> Randomizer = new Randomizer<QbKey>()
            .AddRange(100, QbKey.Left, QbKey.Right, QbKey.Down)
            .AddRange(10, QbKey.A, QbKey.B);

        public ControllerState GetState() => ControllerState.FromKey(Randomizer.Next());
    }
}