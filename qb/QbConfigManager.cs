using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using osu.Framework.Bindables;
using osu.Framework.Platform;

namespace qb
{
    public class QbConfigManager
    {
        const string Filename = "game.json";

        readonly Dictionary<string, IBindable> LoadedBindables = new Dictionary<string, IBindable>();
        readonly Storage Storage;
        readonly JObject JsonData;

        public QbConfigManager(Storage storage)
        {
            Storage = storage;

            using (var stream = storage.GetStream(Filename))
            using (var reader = new StreamReader(stream))
            using (var jreader = new JsonTextReader(reader))
                JsonData = JObject.Load(jreader);
        }

        public void Set<T>(string path, T value)
        {
            var bindable = GetBindable<T>(path);
            bindable.Value = value;
        }
        public T Get<T>(string path, T defaultValue = default!) => GetBindable<T>(path, defaultValue).Value;
        public Bindable<T> GetBindable<T>(string path, T defaultValue = default!)
        {
            if (LoadedBindables.TryGetValue(path, out var loaded) && loaded is Bindable<T> lb) return lb;

            var bindable = new Bindable<T>(defaultValue);
            bindable.ValueChanged += e =>
            {
                var token = JsonData.SelectToken(path);

                var jobj = e.NewValue is { } ? JToken.FromObject(e.NewValue) : (JToken) JValue.CreateNull();
                if (token is { }) token.Replace(jobj);
                else replaceNested(JsonData, path, jobj);


                using (var stream = Storage.GetStream(Filename, FileAccess.Write, FileMode.OpenOrCreate))
                using (var writer = new StreamWriter(stream))
                using (var jwriter = new JsonTextWriter(writer))
                    JsonData.WriteTo(jwriter);
            };

            try
            {
                if (JsonData.SelectToken(path) is { } token && token.ToObject<T>() is { } obj)
                    bindable.Value = obj;
            }
            catch { }

            LoadedBindables[path] = bindable;
            return bindable;




            static void replaceNested(JObject self, string path, JToken value)
            {
                var pathParts = path.Split('.');
                JToken currentNode = self;

                for (int i = 0; i < pathParts.Length; i++)
                {
                    var pathPart = pathParts[i];
                    var isLast = i == pathParts.Length - 1;
                    var partNode = currentNode.SelectToken(pathPart);

                    if (partNode is null)
                    {
                        var nodeToAdd = isLast ? value : new JObject();
                        ((JObject) currentNode).Add(pathPart, nodeToAdd);
                        currentNode = currentNode.SelectToken(pathPart)!;
                    }
                    else
                    {
                        currentNode = partNode;

                        if (isLast)
                            currentNode.Replace(value);
                    }
                }
            }
        }
    }
}