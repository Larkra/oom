using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4.SteamApi
{
    public class SteamApiClientConfiguration
    {
        public string BaseAddress { get; } = "http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/";
        public string MaxQueryResults { get; } = "3";
        public string MaxLength { get; } = "300";
        public string Format { get; } = "json";
    }
}
