using BenchmarkDotNet.Attributes;
using Parsers;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class Parsers
    {
        private const string Line = "2021-02-16,New York,NY,234,asdfdsv,234234,asd";

        private static readonly SplitParser SplitParser = new SplitParser();
        private static readonly ReadParser ReadParser = new ReadParser();

        [Benchmark(Baseline = true)]
        public string Split()
        {
            if (SplitParser.TryGetField(Line, 2, out var value))
            {
                return value;
            }

            return null;
        }

        [Benchmark]
        public string Read()
        {
            if (ReadParser.TryGetField(Line, 2, out var value))
            {
                return value;
            }

            return null;
        }
    }
}