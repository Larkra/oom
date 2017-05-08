using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Task2.Entities;
using Task2.Extensions;

namespace Task2.Services
{
    public class SteamApiClientConfiguration
    {
        public long AppId { get; set; } = 0;
        public string BaseAddress { get; } = "http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/";
        public string MaxQueryResults { get; } = "3";
        public string MaxLength { get; } = "300";
        public string Format { get; } = "json";
    }

    public class SteamApiClient
    {
        private SteamApiClientConfiguration _cfg { get; }

        /// <summary>
        /// Used for Unit Tests as mockable HttpClient
        /// </summary>
        public HttpClient HttpClient { get; set; }

        public SteamApiClient(Action<SteamApiClientConfiguration> setup)
        {
            _cfg = new SteamApiClientConfiguration();
            setup(_cfg);
        }

        public SteamApiClient(SteamApiClientConfiguration configuration)
        {
            _cfg = configuration;
        }

        /// <summary>
        /// See https://developer.valvesoftware.com/wiki/Steam_Web_API for more info...
        /// </summary>
        /// <returns></returns>
        public async Task<List<NewsItem>> GetSteamNewsAsync()
        {
            var newsItems = new List<NewsItem>();

            using (var client = HttpClient ?? new HttpClient())
            {
                var builder = new UriBuilder($"{_cfg.BaseAddress}")
                {
                    Query = new FormUrlEncodedContent(_cfg.ToDictionary()).ReadAsStringAsync().Result
                };
                
                var response = await client.GetAsync(builder.Uri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    newsItems = JObject.Parse(json).SelectToken("appnews.newsitems").ToObject<List<NewsItem>>();
                }
                else
                {
                    // Don't care...
                    return newsItems;
                }

            }

            return newsItems;
        }

    }
}
