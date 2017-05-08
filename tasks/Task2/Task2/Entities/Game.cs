using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Task2.Extensions;
using Task2.Services;

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

        /// <summary>
        /// Gets the Steam Web API AppID of the game
        /// </summary>
        public long AppId { get; }

        /// <summary>
        /// Gets the title of the game
        /// </summary>
        public string Title { get; }

        public Genre Genre { get; }
        
        public Game(long appId, string title, Genre genre)
        {
            AppId = appId < 0 ? 0 : appId;
            Title = title;
            Genre = genre;
            UpdateSteamNews(AppId);
        }

        public override string ToString()
        {
            var ret = $"Title: {Title}\nSteamId: {AppId}\nGenre: {Genre.GetDisplayName()}";

            return ret;
        }

        public void DisplayNews()
        {
            if (!_steamNews.Any())
            {
                Console.WriteLine($"No news available for {Title}");
                return;
            }

            Console.WriteLine($"Latest news for {Title}");
            foreach (var n in _steamNews)
            {
                Console.WriteLine($"{n.ReleaseDate:dd-MM-yyyy HH:mm:ss}");
                Console.WriteLine($"{n.Teaser}");
            }
        }

        /// <summary>
        /// Updates steam news
        /// </summary>
        private void UpdateSteamNews(long appId)
        {
            var client = new SteamApiClient(setup =>
            {
                setup.AppId = appId;
            });

            var appNews = client.GetSteamNewsAsync().Result;

            _steamNews = appNews ?? new List<NewsItem>();
        }
    }
}
