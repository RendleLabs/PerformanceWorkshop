using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Parsers
{
    public static class Aggregator
    {
        private static readonly SplitParser Parser = new();
        
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
    }
}