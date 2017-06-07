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
        Task<IEnumerable<NewsItem>> GetSteamNewsUsingContinueWith(long appId);
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
                Query = await new FormUrlEncodedContent(dict).ReadAsStringAsync()
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

        public Task<IEnumerable<NewsItem>> GetSteamNewsUsingContinueWith(long appId)
        {
            if (appId < 0)
            {
                return Task.Run(() => Enumerable.Empty<NewsItem>());
            }

            var dict = Cfg.ToDictionary();
            dict.Add("appid", appId.ToString());

            /*
             * Holy mother of lambda expressions... await/async 1 <-> continueWith 0
             */
            return new FormUrlEncodedContent(dict).ReadAsStringAsync().ContinueWith(encodedContent => new UriBuilder($"{Cfg.BaseAddress}")
            {
                Query = encodedContent.Result
            }).ContinueWith(uriBuilder =>
                {
                    return Client.GetAsync(uriBuilder.Result.Uri).ContinueWith(responseTask =>
                    {
                        var resp = responseTask.Result;

                        if (!resp.IsSuccessStatusCode)
                        {
                            return Task.Run(() => Enumerable.Empty<NewsItem>());
                        }

                        return resp.Content.ReadAsStringAsync().ContinueWith(jsonTask =>
                        {
                            var json = jsonTask.Result;
                            return JObject.Parse(json).SelectToken("appnews.newsitems").ToObject<IEnumerable<NewsItem>>();
                        });
                    });
                }).Unwrap().Unwrap();
        }

    }
}
