using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Sprites;

namespace qb.Gui
{
    public class PixelText : SpriteText
    {
        public TranslatedString TranslatedText { get => TranslatedTextBindable.Value; set => TranslatedTextBindable.Value = value; }
        readonly Bindable<TranslatedString> TranslatedTextBindable = new();

        public PixelText()
        {
            Current.BindValueChanged(e => Text = e.NewValue?.ToUpperInvariant() ?? string.Empty);
            Font = Font.With(family: "PixelFont", size: 1);

            Truncate = true;
            EllipsisString = "-";
        }

        [BackgroundDependencyLoader]
        void Load(FrameworkConfigManager config)
        {
            var localeb = config.GetBindable<string>(FrameworkSetting.Locale);

            localeb.BindValueChanged(_ => updateText());
            TranslatedTextBindable.BindValueChanged(_ => updateText());
            updateText();

            void updateText()
            {
                if (TranslatedText.IsEmpty) return;
                Text = TranslatedText[localeb.Value];
            }
        }
    }
}