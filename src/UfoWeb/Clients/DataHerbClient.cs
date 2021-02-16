using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Parsers;

namespace UfoWeb.Clients
{
    public class DataHerbClient
    {
        private const string GitHubUrl = "https://raw.githubusercontent.com/DataHerb/nuforc-ufo-records/master/dataset/nuforc_ufo_records.csv";
        private static readonly Uri GitHubUri = new(GitHubUrl, UriKind.Absolute);
        
        private readonly HttpClient _http;

        public DataHerbClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<Dictionary<string, int>> GetAsync()
        {
            await using var stream = await _http.GetStreamAsync(GitHubUri);

            var dict = await Aggregator.UseStreamReader(stream);
            return dict;
        }
    }
}