using System.IO;
using Newtonsoft.Json;
using osu.Framework.Bindables;

namespace qb
{
    public static class Config
    {
        public static readonly Bindable<int> SelectedSkin = new Bindable<int>();

        static int _SelectedSkin;
        static string SavePath;

        static Config()
        {
            SelectedSkin.ValueChanged += _ => Save(SavePath);
        }
        public static void Load(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);

                if (File.Exists(path))
                {
                    var cfg = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(path));
                    _SelectedSkin = cfg.SelectedSkin;
                }
                else
                {
                    _SelectedSkin = 0;
                }
            }
            catch
            {
                _SelectedSkin = 0;
            }

            SavePath = path;
        }
        public static void Save(string? path = null) => File.WriteAllText(path ?? SavePath, JsonConvert.SerializeObject(new Configuration(SelectedSkin.Value)));

        public struct Configuration
        {
            public readonly int SelectedSkin;

            public Configuration(int selectedSkin)
            {
                SelectedSkin = selectedSkin;
            }
        }
    }
}