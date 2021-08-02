using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

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

        public byte[] Serialize(byte version = 1)
        {
            var bytes = new List<byte>(1 + 4 + Frames.Length);
            bytes.Add(version);

            bytes.Add((byte) ((Seed >> 0) & 255));
            bytes.Add((byte) ((Seed >> 8) & 255));
            bytes.Add((byte) ((Seed >> 16) & 255));
            bytes.Add((byte) ((Seed >> 24) & 255));

            if (version == 0) SerializeFramesV0(bytes);
            else if (version == 1) SerializeFramesV1(bytes);

            return bytes.ToArray();


            void SerializeFramesV0(List<byte> bytes)
            {
                foreach (var frame in Frames)
                    bytes.Add(frame.ToBitMask());
            }
            void SerializeFramesV1(List<byte> bytes)
            {
                int count = -1;
                byte prevmask = 0;
                foreach (var frame in Frames)
                {
                    count++;
                    var mask = frame.ToBitMask();

                    if (mask != prevmask || count >= byte.MaxValue)
                    {
                        bytes.Add((byte) count);
                        bytes.Add(prevmask);

                        count = 0;
                        prevmask = mask;
                    }
                }
            };
        }
        public static Replay Deserialize(Stream stream)
        {
            Span<byte> vseed = stackalloc byte[5];
            stream.Read(vseed);

            var version = vseed[0];
            var seed = 0;
            {
                seed |= vseed[1] << 0;
                seed |= vseed[2] << 8;
                seed |= vseed[3] << 16;
                seed |= vseed[4] << 24;
            }

            var frames = version switch
            {
                0 => DeserializeFramesV0(stream),
                1 => DeserializeFramesV1(stream),
                _ => throw new InvalidOperationException(),
            };

            return new Replay(seed, frames.ToImmutableArray());


            static ImmutableArray<ControllerState> DeserializeFramesV0(Stream stream)
            {
                var frames = new List<ControllerState>();
                while (true)
                {
                    var read = ReadByte(stream);
                    if (read is null) break;

                    frames.Add(ControllerState.FromBitMask(read.Value));
                }

                return frames.ToImmutableArray();
            }
            static ImmutableArray<ControllerState> DeserializeFramesV1(Stream stream)
            {
                var frames = new List<ControllerState>();
                while (true)
                {
                    var count = ReadByte(stream);
                    if (count is null) break;

                    var mask = ReadByte(stream);
                    if (mask is null) break;

                    var state = ControllerState.FromBitMask(mask.Value);
                    for (int i = 0; i < count; i++) frames.Add(state);
                }

                return frames.ToImmutableArray();
            }

            static byte? ReadByte(Stream stream)
            {
                Span<byte> span = stackalloc byte[1];
                var read = stream.Read(span);
                if (read == 0) return null;

                return span[0];
            }
        }

    }
}