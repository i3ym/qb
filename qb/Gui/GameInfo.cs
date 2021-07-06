using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;

namespace qb.Gui
{
    public class GameInfo : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        void Load(FieldController controller)
        {
            AddInternal(GetNewText(1.5f, controller.Level));
            AddInternal(GetNewText(4.5f, controller.Score));
            AddInternal(GetNewText(7.5f, controller.Time));
            AddInternal(GetNewText(10.5f, controller.Lines));

            AddInternal(GetNewText(0, ("Level", "Уровень")));
            AddInternal(GetNewText(3, ("Score", "Очки")));
            AddInternal(GetNewText(6, ("Time", "Время")));
            AddInternal(GetNewText(9, ("Lines", "Линии")));
        }

        PixelText GetNewText<T>(float y, Bindable<T> source)
        {
            var tb = GetNewText(y);
            source.BindValueChanged(e => Schedule(() => tb.Text = e.NewValue?.ToString() ?? string.Empty), true);

            return tb;
        }
        PixelText GetNewText(float y, TranslatedString text)
        {
            var tb = GetNewText(y);
            tb.TranslatedText = text;

            return tb;
        }
        PixelText GetNewText(float y) => new PixelText() { Height = 1, Width = 7, X = 1.5f, Y = y + .5f };
    }
}