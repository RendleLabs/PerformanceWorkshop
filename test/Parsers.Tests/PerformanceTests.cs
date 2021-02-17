using NBench;
using Pro.NBench.xUnit.XunitExtensions;
using Xunit;

namespace Parsers.Tests
{
    public class PerformanceTests
    {
        private const string Line =
            "6/1/44 12:00,Los Alamos,NM,Disk,1+ Hour,Disk hovers over Los Alamos during the development of the atomic bombs; pursued by planes.,5/12/09";
        
        [NBenchFact(Skip = "Too slow")]
        [PerfBenchmark(Description = "Parse using Split", RunMode = RunMode.Iterations, TestMode = TestMode.Test)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 1)]
        public void SplitParserWorks()
        {
            var target = new SplitParser();
            for (int i = 0; i < 10000; i++)
            {
                Assert.True(target.TryGetField(Line, 2, out var actual));
                Assert.Equal("NM", actual);
            }
        }
        
        [NBenchFact]
        [PerfBenchmark(Description = "Parse using Read", RunMode = RunMode.Iterations, TestMode = TestMode.Test)]
        [GcTotalAssertion(GcMetric.TotalCollections,
            GcGeneration.Gen1,
            MustBe.ExactlyEqualTo,
            0)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 1)]
        public void ReadParserWorks()
        {
            var target = new ReadParser();
            for (int i = 0; i < 10000; i++)
            {
                Assert.True(target.TryGetField(Line, 2, out var actual));
                Assert.Equal("NM", actual);
            }
        }
    }
}