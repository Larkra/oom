using System.Collections.Generic;
using Task4.Entities;
using Task4.SteamApi;

namespace Task4.Services
{
    public interface INewsService
    {
        IEnumerable<NewsItem> GetNewsById(long appId);
    }

    public class NewsService : INewsService
    {
        private readonly ISteamApiClient _client;

        public NewsService(ISteamApiClient steamApiClient)
        {
            _client = steamApiClient;
        }

        public IEnumerable<NewsItem> GetNewsById(long appId)
        {
            return _client.GetSteamNewsAsync(appId).Result;
        }
    }
}
