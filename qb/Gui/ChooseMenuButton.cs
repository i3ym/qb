namespace qb.Gui
{
    public interface IChooseMenuButton
    {
        bool IsActive { get; }
    }
    public partial class ChooseMenuButton<T> : ValueMenuButton<T>, IChooseMenuButton
    {
        public bool IsActive => EqualityComparer<T>.Default.Equals(Source.Value, Value);

        readonly Bindable<T> Source;

        public ChooseMenuButton(T value, Bindable<T> source) : base(value) => Source = source.GetBoundCopy();
        public ChooseMenuButton(T value, Bindable<T> source, TranslatedString text) : base(value, text) => Source = source.GetBoundCopy();
        public ChooseMenuButton(T value, Bindable<T> source, TranslatedString text, Action action) : base(value, text, action) => Source = source.GetBoundCopy();

        [BackgroundDependencyLoader]
        void Load()
        {
            Source.BindValueChanged(ValueChanged);
            Action += () => Source.Value = Value;
            ValueChanged(Source.Value, 0);
        }

        void ValueChanged(ValueChangedEvent<T> e) => ValueChanged(e.NewValue);
        void ValueChanged(T value, double duration = 250) => PixelText.FadeColour(object.Equals(value, Value) ? Colour4.White : Colour4.Gray, duration, Easing.OutQuad);
    }
}