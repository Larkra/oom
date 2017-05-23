using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using Task4.Entities;
using Task4.Extensions;
using Task4.Repositories;

namespace Task4.Services
{
    public interface IGameService
    {
        void AddGame(Game newGame);
        void AddGames(List<Game> games);
        void UpdateGameInfo(Game game);
        void DeleteGame(Game game);


        void DisplayGameInfo(Game game);
        void DisplayGameNews(Game game);
        void UpdateGameNews(Game game);

        IList<Game> GetAllGames();
        Game GetGameById(long appId);
        IList<Game> GetGamesByGenre(Genre genre);
    }

    public class GameService : IGameService
    {
        private readonly INewsService _newsService;
        private readonly IGameRepository _gamesRepo;

        private static List<Game> _allGames;

        private List<Game> AllGames
        {
            get
            {
                if (_allGames == null)
                {
                    _allGames = _gamesRepo.GetAllGames().ToList();
                }
                return _allGames;
            }
        }

        public GameService(INewsService newsService
            , IGameRepository gamesRepository)
        {
            _newsService = newsService;
            _gamesRepo = gamesRepository;
        }

        public void DisplayGameInfo(Game game)
        {
            WriteLine($"Title: {game.Title}\nGenre: {game.Genre.GetDisplayName()}{(game.AppId > 0 ? "\nAppId: " + game.AppId : string.Empty)}");
        }

        #region SteamNews
        public void DisplayGameNews(Game game)
        {
            if (!game.NewsItems.Any())
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"No news available for {game.Title}");
                ResetColor();
                return;
            }

            WriteLine($"Latest news for {game.Title}");
            foreach (var newsItem in game.NewsItems)
            {
                WriteLine($"{newsItem.ReleaseDate:dd-MM-yyyy HH:mm:ss} {newsItem.NewsTitle}");
                WriteLine($"\t{newsItem.Teaser}");
            }
        }

        public void UpdateGameNews(Game game)
        {
            var newsItems = _newsService.GetNewsById(game.AppId);

            var distinctNewsItems = newsItems.Union(game.NewsItems)
                                             .DistinctBy(n => n.NewsGuid)
                                             .OrderByDescending(n => n.ReleaseDate);

            game.NewsItems = distinctNewsItems.Take(3);
        }
        #endregion

        #region GetMethods
        public IList<Game> GetAllGames()
        {
            return AllGames;
        }

        public Game GetGameById(long appId)
        {
            //Normally this should be done by the repository, but for the sake of some Unit Tests...
            return AllGames.FirstOrDefault(g => g.AppId == appId);
        }

        public IList<Game> GetGamesByGenre(Genre genre)
        {
            return AllGames.Where(g => g.Genre == genre).ToList();
        }
        #endregion

        public void AddGame(Game newGame)
        {
            ValidateGame(newGame);

            AllGames.Add(newGame);
            _gamesRepo.Save(AllGames);
        }

        public void AddGames(List<Game> games)
        {
            ValidateGames(games);

            _gamesRepo.Save(games);
        }

        public void UpdateGameInfo(Game game)
        {
            var idx = AllGames.FindIndex(g => g.AppId == game.AppId);
            if (idx == -1)
            {
                WriteLine($"Could not update game because the given AppId was not found.\nTitle: [{game.Title}]\nAppId [{game.AppId}]");
                return;
            }
            ValidateGame(game, false);

            AllGames[idx] = game;
            _gamesRepo.Save(AllGames);
        }

        public void DeleteGame(Game game)
        {
            var idx = AllGames.FindIndex(g => g.AppId == game.AppId);
            if (idx == -1)
            {
                WriteLine($"Game to delete was not found.\nTitle: [{game.Title}]\nAppId [{game.AppId}]");
                return;
            }

            AllGames.RemoveAt(idx);
            _gamesRepo.Save(AllGames);
        }

        #region Validation
        private void ValidateGames(List<Game> games)
        {
            games.ForEach(g => ValidateGame(g));
        }

        private void ValidateGame(Game game, bool validateDuplicates = true)
        {
            if (game.AppId < 0)
            {
                throw new ArgumentException("AppId must not be negative", nameof(game.AppId));
            }
            if (string.IsNullOrWhiteSpace(game.Title))
            {
                throw new ArgumentException("Title must not be null or empty", nameof(Title));
            }
            if (validateDuplicates && AllGames.Any(g => g.AppId == game.AppId))
            {
                throw new ArgumentException($"Found duplicate AppId [{game.AppId}] for {game.Title}");
            }
        }
        #endregion

    }

}
