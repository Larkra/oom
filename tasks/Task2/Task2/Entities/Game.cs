using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Task2.Extensions;
using Task2.Services;
using static System.Console;

namespace Task2.Entities
{
    #region Enum Genre
    public enum Genre
    {
        [Display(Name = "Unknown Genre", GroupName = "Unknown")]
        Unknown,

        [Display(Name = "Construction and management simulation", GroupName = "Simulation")]
        CityBuilder,

        [Display(Name = "Point & Click Adventure", GroupName = "Adventure")]
        PointAndClick,

        [Display(Name = "Graphic Adventure", GroupName = "Adventure")]
        GraphicAdventure,

        [Display(Name = "Text Adventure", GroupName = "Adventure")]
        TextAdventure,

        [Display(Name = "Shooter", GroupName = "Action")]
        Shooter,

        [Display(Name = "Stealth", GroupName = "Action")]
        Stealth,

        [Display(Name = "Massively multiplayer online role-playing games", GroupName = "Role-Playing")]
        Mmorpg,

        [Display(Name = "4x Strategy", GroupName = "Strategy")]
        Strategy4X
    }
    #endregion

    public class Game
    {
        private IList<NewsItem> _steamNews = new List<NewsItem>();
        private string _title;

        #region properties
        /// <summary>
        /// Gets the Steam Web API AppID of the game
        /// </summary>
        public long AppId { get; }

        /// <summary>
        /// Gets the title of the game
        /// </summary>
        public string Title
        {
            get { return _title; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
                _title = value;
            }
        }

        /// <summary>
        /// Gets the genre of the game
        /// </summary>
        public Genre Genre { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the game may be available on Steam
        /// </summary>
        public bool IsSteamApp => AppId > 0;

        /// <summary>
        /// Gets count of the available news items
        /// </summary>
        public int CountNewsAvailable => _steamNews.Count;
        #endregion

        /// <summary>
        /// Creates a new game object which is not available on Steam
        /// </summary>
        /// <param name="title">The title of the game</param>
        /// <param name="genre">The genre of the game</param>
        public Game(string title, Genre genre)
        {
            Title = title;
            Genre = genre;
        }

        /// <summary>
        /// Creates a new game object which has a AppId and therefore is available on Steam
        /// </summary>
        /// <param name="appId">The unique Steam AppId of the game</param>
        /// <param name="title">The title of the game</param>
        /// <param name="genre">The genre of the game</param>
        public Game(long appId, string title, Genre genre)
        {
            if (appId < 0) throw new ArgumentException("AppId must not be negative.", nameof(appId));

            AppId = appId;
            Title = title;
            Genre = genre;
        }

        public override string ToString()
        {
            var ret = $"Title: {Title}\nGenre: {Genre.GetDisplayName()}{(IsSteamApp ? "\nAppId: " + AppId : string.Empty)}";

            return ret;
        }

        /// <summary>
        /// Displays Steam news for the game if available
        /// </summary>
        public void DisplayNews()
        {
            if (!_steamNews.Any())
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"No news available for {Title}");
                ResetColor();
                return;
            }

            WriteLine($"Latest news for {Title}");
            foreach (var newsItem in _steamNews)
            {
                WriteLine($"\t{newsItem.ReleaseDate:dd-MM-yyyy HH:mm:ss}");
                WriteLine($"\t{newsItem.Teaser}");
            }
        }

        /// <summary>
        /// Gets the latest news from Steam
        /// </summary>
        public void GetLatestSteamNews()
        {
            if (!IsSteamApp) return;

            var client = new SteamApiClient(setup =>
            {
                setup.AppId = AppId;
            });
            
            var latestNews = client.GetSteamNewsAsync().Result;

            _steamNews = latestNews ?? new List<NewsItem>();
        }

    }
}
