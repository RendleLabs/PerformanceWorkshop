using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Parsers;

namespace ProfileApp
{
    class Program
    {
        private static readonly SplitParser Parser = new();

        static async Task Main(string[] args)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csvPath = Path.Combine(directory!, "Data", "nuforc_ufo_records.csv");
            await using var stream = File.OpenRead(csvPath);

            var states = await Aggregator.UseStreamReader(stream);
            
            foreach (var state in states.Keys.OrderBy(k => k))
            {
                Console.WriteLine($"{state}: {states[state]}");
            }
        }
    }
}