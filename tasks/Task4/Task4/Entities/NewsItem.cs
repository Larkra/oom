using System;
using System.Net;
using Newtonsoft.Json;
using Task4.Utilities;

namespace Task4.Entities
{
    public class NewsItem
    {
        /*
         * See
         * http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid=440&count=3&maxlength=300&format=json 
         * for a complete list of available result values 
         */

        private const int TeaserLength = 100;

        [JsonIgnore]
        private string _contentsDecoded;

        public string Contents
        {
            get { return _contentsDecoded; }
            set { _contentsDecoded = WebUtility.HtmlDecode(value); }
        }

        [JsonIgnore]
        public string Teaser => _contentsDecoded.Substring(0, Math.Min(_contentsDecoded.Length, TeaserLength)) + "[...]";

        [JsonProperty("Gid")]
        public long NewsGuid { get; set; }

        [JsonProperty("title")]
        public string NewsTitle { get; set; }
        
        [JsonProperty("date")]
        [JsonConverter(typeof(ReleaseDateDateTimeConverter))]
        public DateTime ReleaseDate { get; set; }
    }
}
