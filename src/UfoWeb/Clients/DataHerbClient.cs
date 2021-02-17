using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Timer;
using Microsoft.Extensions.Caching.Memory;
using Parsers;

namespace UfoWeb.Clients
{
    public class DataHerbClient
    {
        private static readonly ActivitySource Activity = new(typeof(DataHerbClient).FullName!);

        private static readonly TimerOptions ParseTimer = new()
        {
            MeasurementUnit = Unit.Calls,
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds,
            Name = "DataHerbClientParse"
        };

        private const string CacheKey = "DataHerbClient";
        private const string GitHubUrl = "https://raw.githubusercontent.com/DataHerb/nuforc-ufo-records/master/dataset/nuforc_ufo_records.csv";
        private static readonly Uri GitHubUri = new(GitHubUrl, UriKind.Absolute);
        private static readonly TimeSpan CacheTime = TimeSpan.FromSeconds(5);
        
        private readonly HttpClient _http;
        private readonly IMetrics _metrics;
        private readonly IMemoryCache _cache;

        public DataHerbClient(HttpClient http, IMetrics metrics, IMemoryCache cache)
        {
            _http = http;
            _metrics = metrics;
            _cache = cache;
        }

        public ValueTask<Dictionary<string, int>> GetAsync()
        {
            if (_cache.TryGetValue<Dictionary<string, int>>(CacheKey, out var value))
            {
                return new ValueTask<Dictionary<string, int>>(value);
            }

            return new ValueTask<Dictionary<string, int>>(GetImpl());
        }

        public async Task<Dictionary<string, int>> GetImpl()
        {
            await using var stream = await _http.GetStreamAsync(GitHubUri);

            using (_metrics.Measure.Timer.Time(ParseTimer))
            {
                using (Activity.StartActivity("Parse"))
                {
                    var dict = await Aggregator.UsePipelines(stream);
                    _cache.Set(CacheKey, dict, CacheTime);
                    return dict;
                }
            }
        }
    }
}