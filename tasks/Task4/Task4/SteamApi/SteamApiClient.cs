using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Task4.Entities;
using Task4.Extensions;

namespace Task4.SteamApi
{
    public interface ISteamApiClient
    {
        Task<IEnumerable<NewsItem>> GetSteamNewsAsync(long appId);
    }

    public class SteamApiClient : ISteamApiClient
    {
        /*
         * https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
         */
        private static readonly HttpClient Client = new HttpClient();

        private SteamApiClientConfiguration Cfg { get; }

        public SteamApiClient()
        {
            Cfg = new SteamApiClientConfiguration();
        }

        public SteamApiClient(Action<SteamApiClientConfiguration> setup)
        {
            Cfg = new SteamApiClientConfiguration();
            setup(Cfg);
        }

        public SteamApiClient(SteamApiClientConfiguration configuration)
        {
            Cfg = configuration;
        }

        /// <summary>
        /// See https://developer.valvesoftware.com/wiki/Steam_Web_API for more info...
        /// </summary>
        /// <returns>A list of newsitems</returns>
        public async Task<IEnumerable<NewsItem>> GetSteamNewsAsync(long appId)
        {
            if (appId < 0)
            {
                return Enumerable.Empty<NewsItem>();
            }

            var dict = Cfg.ToDictionary();
            dict.Add("appid", appId.ToString());

            var builder = new UriBuilder($"{Cfg.BaseAddress}")
            {
                Query = new FormUrlEncodedContent(dict).ReadAsStringAsync().Result
            };

            using (var response = await Client.GetAsync(builder.Uri))
            {
                if (!response.IsSuccessStatusCode)
                {
                    // Don't care, cause this is just an example and I am lazy...
                    return Enumerable.Empty<NewsItem>();
                }
                var json = await response.Content.ReadAsStringAsync();

                return JObject.Parse(json).SelectToken("appnews.newsitems").ToObject<List<NewsItem>>();
            }
        }
    }
}
