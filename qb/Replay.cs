using System.Collections.Immutable;
using System.IO;
using Newtonsoft.Json;

namespace qb
{
    public class Replay
    {
        public readonly int Seed;
        public readonly ImmutableArray<ControllerState> Frames;

        public Replay(int seed, ImmutableArray<ControllerState> frames)
        {
            Seed = seed;
            Frames = frames;
        }

        public string AsJson() => JsonConvert.SerializeObject(this);
        public static Replay FromJson(Stream stream) => FromJson(new StreamReader(stream).ReadToEnd());
        public static Replay FromJson(string json) => JsonConvert.DeserializeObject<Replay>(json)!;
    }
}