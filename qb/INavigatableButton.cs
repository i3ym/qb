namespace qb
{
    public interface INavigatableButton : IDrawable
    {
        bool Focusable { get; }

        void OnEvent(QbKey action);
    }
}