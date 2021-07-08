using osu.Framework.Platform;

namespace qb
{
    public class QbConfigManager : JsonConfigManager<QbConfigField>
    {
        public QbConfigManager(Storage storage) : base(storage)
        {
            SetDefault(QbConfigField.SelectedSkin, 0, min: 0);
            Load();
        }
    }
}