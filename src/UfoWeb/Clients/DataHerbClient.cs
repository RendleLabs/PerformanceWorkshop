using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Parsers;

namespace UfoWeb.Clients
{
    public class DataHerbClient
    {
        private static readonly ActivitySource Activity = new(typeof(DataHerbClient).FullName!);
        
        private const string CacheKey = "DataHerbClient";
        private static readonly TimeSpan CacheTime = TimeSpan.FromMinutes(5);
        private const string GitHubUrl = "https://raw.githubusercontent.com/DataHerb/nuforc-ufo-records/master/dataset/nuforc_ufo_records.csv";
        private static readonly Uri GitHubUri = new(GitHubUrl, UriKind.Absolute);
        
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;

        public DataHerbClient(HttpClient http, IMemoryCache cache)
        {
            _http = http;
            _cache = cache;
        }

        public ValueTask<Dictionary<string, int>> GetAsync()
        {
            return _cache.TryGetValue<Dictionary<string, int>>(CacheKey, out var cached)
                ? new ValueTask<Dictionary<string, int>>(cached)
                : new ValueTask<Dictionary<string, int>>(GetImpl());
        }

        private async Task<Dictionary<string, int>> GetImpl()
        {
            await using var stream = await _http.GetStreamAsync(GitHubUri);

            Dictionary<string, int> dict;
            
            using (var activity = Activity.StartActivity("Parse"))
            {
                dict = await Aggregator.UseStreamReader(stream);
            }
            _cache.Set(CacheKey, dict, CacheTime);
            return dict;
        }
    }
}