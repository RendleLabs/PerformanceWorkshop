using System;
using System.Collections;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Returning>();
            Console.WriteLine(summary.LogFilePath);
        }
    }

    [DisassemblyDiagnoser]
    public class Returning
    {
        private static readonly Random Random = new Random();

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public int JustReturn(int a, int b) => a + b;

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public int VarAndReturn(int a, int b)
        {
            int r = a + b;
            return r;
        }

        public IEnumerable<object[]> Data()
        {
            yield return new object[] {1, 1};
            yield return new object[] {2, 2};
            yield return new object[] {10, 10};
            yield return new object[] {42, 42};
        }
    }
}
