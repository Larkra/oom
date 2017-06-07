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
            /* Oh noes, we are blocking!
             * 
             * Yes we are, because else we would have to expand async through the whole codebase,
             * until we reach the Main method, which of course cannot be async.
             * 
             * In other words:
             * It's okay to use .Result in the context of an console application, at least in main method.
             * But again - I am lazy, so I am blocking it here...
             * 
             * For more info on the topic
             * https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
             * 
             * Furthermore, info about the upcoming Async Main in C# 7.1
             * https://blogs.msdn.microsoft.com/mazhou/2017/05/30/c-7-series-part-2-async-main/
             */
             
            return _client.GetSteamNewsUsingContinueWith(appId).Result;
        }

    }
}
