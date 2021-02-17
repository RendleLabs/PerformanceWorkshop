using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{
    public static class Aggregator
    {
        private static readonly ReadParser Parser = new();

        public static async Task<Dictionary<string, int>> UseStreamReader(Stream stream)
        {
            var output = new Dictionary<string, int>();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(line)) continue;

                if (Parser.TryGetField(line, 2, out var state))
                {
                    output.TryGetValue(state, out int count);
                    output[state] = ++count;
                }
            }

            return output;
        }

        public static async Task<Dictionary<string, int>> UsePipelines(Stream stream)
        {
            var output = new Dictionary<string, int>();

            var pipe = PipeReader.Create(stream);

            while (true)
            {
                var result = await pipe.ReadAsync();
                var position = Read(result.Buffer, result.IsCompleted, output);

                if (result.IsCompleted) break;

                pipe.AdvanceTo(position, result.Buffer.End);
            }

            await pipe.CompleteAsync();

            return output;
        }

        private static SequencePosition Read(ReadOnlySequence<byte> buffer, bool resultIsCompleted, Dictionary<string, int> output)
        {
            const byte newline = (byte) '\n';

            var reader = new SequenceReader<byte>(buffer);

            while (!reader.End)
            {
                if (reader.TryReadTo(out ReadOnlySpan<byte> bytes, newline))
                {
                    ParseLine(bytes, output);
                }
                else if (resultIsCompleted)
                {
                    var slice = buffer.Slice(reader.Position);
                    ParseLine(slice.FirstSpan, output);
                }
                else
                {
                    break;
                }
            }

            return reader.Position;
        }

        private static void ParseLine(ReadOnlySpan<byte> bytes, Dictionary<string, int> output)
        {
            var chars = ArrayPool<char>.Shared.Rent(bytes.Length);

            try
            {
                int length = Encoding.UTF8.GetChars(bytes, chars);
                if (Parser.TryGetField(chars.AsSpan().Slice(0, length), 2,
                    out var state))
                {
                    output.TryGetValue(state, out int count);
                    output[state] = ++count;
                }
            }
            finally
            {
                ArrayPool<char>.Shared.Return(chars);
            }
        }
    }
}