using System.Collections.Generic;
using Task4.SteamApi;

namespace Task4.Extensions
{
    public static class SteamApiClientConfigurationExtensions
    {
        public static IDictionary<string, string> ToDictionary(this SteamApiClientConfiguration apiClientConfiguration)
        {
            var dict = new Dictionary<string, string>();
            
            if (!string.IsNullOrWhiteSpace(apiClientConfiguration.MaxLength))
            {
                dict.Add("maxlength", apiClientConfiguration.MaxLength);
            }
            if (!string.IsNullOrWhiteSpace(apiClientConfiguration.MaxQueryResults))
            {
                dict.Add("count", apiClientConfiguration.MaxQueryResults);
            }
            if (!string.IsNullOrWhiteSpace(apiClientConfiguration.Format))
            {
                dict.Add("format", apiClientConfiguration.Format);
            }

            return dict;
        }
    }
}
