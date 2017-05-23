using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ShellProgressBar;
using static System.Console;
using Task4.Entities;
using Task4.Extensions;
using Task4.Services;
using Task4.SteamApi;
using Task4.Utilities;
using Task4.Repositories;

namespace Task4
{
    class Program
    {
        private static IServiceProvider _provider;

        #region GamesMock

        private static IEnumerable<Game> GamesMock
        {
            get
            {
                var mock = new List<Game>
                {
                    new Game {AppId = 255710, Title = "Cities Skylines", Genre = Genre.CityBuilder},
                    new Game {AppId = 388210, Title = "Day of the Tentacle Remastered", Genre = Genre.PointAndClick},
                    new Game {AppId = 250740, Title = "Ragnarok Online", Genre = Genre.Mmorpg},
                    new Game {AppId = 289130, Title = "Endless Legend", Genre = Genre.Strategy4X},
                    new Game {AppId = 901660, Title = "Sam & Max: Season One", Genre = Genre.GraphicAdventure},
                    new Game {AppId = 32340, Title = "LOOM", Genre = Genre.PointAndClick}
                };

                return mock;
            }
        }
        #endregion

        static void Main(string[] args)
        {
            _provider = RegisterServices();
            
            RunNewsUpdate();
            RunGameInfoUpdate();
            RunDeleteGame();
        }

        private static void RunNewsUpdate()
        {
            var gameService = _provider.GetService<IGameService>();

            var games = gameService.GetAllGames().ToList();
            if (!games.Any())
            {
                games = GamesMock.ToList();
                gameService.AddGames(games);
            }

            foreach (var game in games.OrderBy(g => g.Genre.GetGroupName()))
            {
                WriteLine();
                gameService.DisplayGameInfo(game);
            }
            RefreshShell();

            WriteLine("\nLatest news for games available on Steam before news update...\n");
            foreach (var game in games)
            {
                gameService.DisplayGameNews(game);
            }
            RefreshShell();

            WriteLine("\nTrying to get latest news for games available on Steam...\n");
            using (var pbar = new ProgressBar(games.Count, "Starting ", ConsoleColor.DarkGreen))
            {
                foreach (var g in games)
                {
                    pbar.Tick($"Currently processing {g.Title}");
                    gameService.UpdateGameNews(g);
                }
            }
            RefreshShell();

            WriteLine("\nLatest news for all games after news update...\n");
            foreach (var g in games)
            {
                gameService.DisplayGameNews(g);
            }
        }

        private static void RunDeleteGame()
        {
            var gameService = _provider.GetService<IGameService>();

            var gamesBeforeDelete = gameService.GetAllGames().ToList();
            if (!gamesBeforeDelete.Any())
            {
                gamesBeforeDelete = GamesMock.ToList();
                gameService.AddGames(gamesBeforeDelete);
            }

            WriteLine("Before delete:");
            foreach (var game in gamesBeforeDelete.OrderBy(g => g.Genre.GetGroupName()))
            {
                WriteLine();
                gameService.DisplayGameInfo(game);
            }
            RefreshShell();
            
            var gameToDelete = gamesBeforeDelete.FirstOrDefault();
            
            gameService.DeleteGame(gameToDelete);

            WriteLine("After delete:");
            var gamesAfterDelete = gameService.GetAllGames().ToList();
            foreach (var game in gamesAfterDelete.OrderBy(g => g.Genre.GetGroupName()))
            {
                WriteLine();
                gameService.DisplayGameInfo(game);
            }
            RefreshShell();
        }

        private static void RunGameInfoUpdate()
        {
            var gameService = _provider.GetService<IGameService>();

            var games = gameService.GetAllGames().ToList();
            if (!games.Any())
            {
                games = GamesMock.ToList();
                gameService.AddGames(games);
            }

            foreach (var game in games.OrderBy(g => g.Genre.GetGroupName()))
            {
                WriteLine();
                gameService.DisplayGameInfo(game);
            }
            RefreshShell();

            var gameToUpdate = games.FirstOrDefault();

            WriteLine("Before GameInfo update:");
            gameService.DisplayGameInfo(gameToUpdate);

            if (gameToUpdate != null)
            {
                gameToUpdate.Genre = Genre.Strategy4X;
                gameToUpdate.Title = "Updated!";

                gameService.UpdateGameInfo(gameToUpdate);

                WriteLine("After GameInfo update:");
                gameService.DisplayGameInfo(gameToUpdate);
            }
            RefreshShell();

            foreach (var game in games.OrderBy(g => g.Genre.GetGroupName()))
            {
                WriteLine();
                gameService.DisplayGameInfo(game);
            }
            RefreshShell();
        }
        
        private static IServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ISteamApiClient, SteamApiClient>();

            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IFileHandler, FileHandler>();
            services.AddTransient<INewsService, NewsService>();
            services.AddTransient<IGameService, GameService>();
            
            return services.BuildServiceProvider();
        }

        private static readonly Action RefreshShell = () =>
        {
            WriteLine("\nPress any key to continue...");
            ReadKey();
            Clear();
        };

    }
}
