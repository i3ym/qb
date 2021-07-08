using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace qb
{
    public class JsonConfigManager<T> : ConfigManager<T> where T : struct, Enum
    {
        protected virtual string Filename => "game.json";

        private readonly Storage Storage;

        public JsonConfigManager(Storage storage, IDictionary<T, object>? defaultOverrides = null) : base(defaultOverrides) =>
            Storage = storage;

        protected override void PerformLoad()
        {
            using var stream = Storage.GetStream(Filename);
            if (stream is null) return;

            var json = JObject.Load(new JsonTextReader(new StreamReader(stream)));

            foreach (var (key, value) in json)
            {
                if (value is null) continue;
                if (!Enum.TryParse<T>(key, out var k))
                    continue;

                if (ConfigStore.TryGetValue(k, out var bindable))
                {
                    if (bindable is IParseable parseable)
                        parseable.Parse(value.Value<string>());

                    continue;
                }

                if (value.Type == JTokenType.String) SetDefault(k, value.Value<string>());
                else if (value.Type == JTokenType.Integer) SetDefault(k, value.Value<int>());
                else if (value.Type == JTokenType.Float) SetDefault(k, value.Value<float>());
                else if (value.Type == JTokenType.Boolean) SetDefault(k, value.Value<bool>());
                else if (value.Type == JTokenType.Date) SetDefault(k, value.Value<DateTime>());
                else if (value.Type == JTokenType.TimeSpan) SetDefault(k, value.Value<TimeSpan>());
            }
        }
        protected override bool PerformSave()
        {
            using var stream = new StreamWriter(Storage.GetStream(Filename, FileAccess.Write, FileMode.Create));
            stream.Write(JsonConvert.SerializeObject(ConfigStore, Formatting.Indented));

            return true;
        }
    }
}