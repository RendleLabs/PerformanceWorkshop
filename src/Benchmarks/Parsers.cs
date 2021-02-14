using System.Text;
using BenchmarkDotNet.Attributes;
using Parsers;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class Parsers
    {
        private const string Line =
            "6/1/44 12:00,Los Alamos,NM,Disk,1+ Hour,Disk hovers over Los Alamos during the development of the atomic bombs; pursued by planes.,5/12/09";

        private static readonly byte[] LineBytes = Encoding.UTF8.GetBytes(Line);
        
        private static readonly SplitParser Splitter = new();
        private static readonly ReadParser Reader = new();

        [Benchmark(Baseline = true)]
        public string? Split() => Splitter.TryGetField(Line, 2, out var value) ? value : null;

        [Benchmark]
        public string? Read() => Reader.TryGetField(Line, 2, out var value) ? value : null;

        [Benchmark]
        public string? ReadBytes() => Reader.TryGetField(LineBytes, 2, out var value) ? value : null;
    }
}