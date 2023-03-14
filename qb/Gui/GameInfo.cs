namespace qb.Gui
{
    public partial class GameInfo : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        void Load(FieldController controller)
        {
            AddInternal(GetNewText(1.2f, controller.Level, 1.2f));
            AddInternal(GetNewText(5.2f, controller.Score, 1.2f));
            AddInternal(GetNewText(8.2f, controller.Frame, 1.2f, t => TimeSpan.FromMilliseconds(t * FieldController.FrameInterval).ToString(@"mm\:ss")));
            AddInternal(GetNewText(11.2f, controller.Lines, 1.2f));

            AddInternal(GetNewText(0, ("Level", "Уровень")));
            AddInternal(GetNewText(4, ("Score", "Очки")));
            AddInternal(GetNewText(7, ("Time", "Время")));
            AddInternal(GetNewText(10, ("Lines", "Линии")));
        }

        PixelText GetNewText<T>(float y, Bindable<T> source, float fontsize = .8f, Func<T, string>? formatfunc = null)
        {
            formatfunc ??= e => e?.ToString() ?? string.Empty;

            var tb = GetNewText(y, fontsize);
            source.BindValueChanged(e => tb.Text = formatfunc(e.NewValue), true);

            return tb;
        }
        PixelText GetNewText(float y, TranslatedString text, float fontsize = .8f)
        {
            var tb = GetNewText(y, fontsize);
            tb.TranslatedText = text;

            return tb;
        }
        PixelText GetNewText(float y, float fontsize) => new PixelText() { Height = 1, Width = 7, X = 1.5f, Y = y + .5f, FontSize = fontsize };
    }
}