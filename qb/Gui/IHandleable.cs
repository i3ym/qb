namespace qb.Gui
{
    public interface IHandleable : IDrawable
    {
        bool HandleEvents { get; set; }
    }
}