using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace qb.Gui
{
    public class GameInfo : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        void Load(FieldController controller)
        {
            AddInternal(GetNewText(1.5f, controller.Level).With(x => x.Font = x.Font.With(size: 2)));
            AddInternal(GetNewText(5.5f, controller.Score));
            AddInternal(GetNewText(8.5f, controller.Frame, t => TimeSpan.FromMilliseconds(t * FieldController.FrameInterval).ToString(@"mm\:ss")));
            AddInternal(GetNewText(11.5f, controller.Lines));

            AddInternal(GetNewText(0, ("Level", "Уровень")));
            AddInternal(GetNewText(4, ("Score", "Очки")));
            AddInternal(GetNewText(7, ("Time", "Время")));
            AddInternal(GetNewText(10, ("Lines", "Линии")));
        }

        PixelText GetNewText<T>(float y, Bindable<T> source, Func<T, string>? formatfunc = null)
        {
            formatfunc ??= e => e?.ToString() ?? string.Empty;

            var tb = GetNewText(y);
            source.BindValueChanged(e => tb.Text = formatfunc(e.NewValue), true);

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