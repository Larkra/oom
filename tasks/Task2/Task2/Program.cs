using System;
using System.Collections.Generic;
using System.Linq;
using ShellProgressBar;
using Task2.Entities;
using Task2.Extensions;
using static System.Console;

namespace Task2
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            WriteLine();
        }
        
        protected static void Run()
        {
            var games = new List<Game>
            {
                new Game(255710 , "Cities Skylines",                Genre.CityBuilder),
                new Game(388210 , "Day of the Tentacle Remastered", Genre.PointAndClick),
                new Game(250740 , "Ragnarok Online",                Genre.Mmorpg),
                new Game(289130 , "Endless Legend",                 Genre.Strategy4X),
                new Game(         "Zork",                           Genre.TextAdventure),
                new Game(901660 , "Sam & Max: Season One",          Genre.GraphicAdventure),
                new Game(32340  , "LOOM",                           Genre.PointAndClick)
            };

            foreach (var game in games.OrderBy(g => g.Genre.GetGroupName()))
            {
                WriteLine();
                WriteLine($"{game}");
            }
            RefreshShell();

            var steamGames = games.Where(g => g.IsSteamApp).ToList();

            WriteLine("\nLatest news for games available on Steam before news update...\n");
            foreach (var game in steamGames)
            {
                game.DisplayNews();
            }
            RefreshShell();

            WriteLine("\nTrying to get latest news for games available on Steam...\n");
            using (var pbar = new ProgressBar(steamGames.Count, "Starting ", ConsoleColor.DarkGreen))
            {
                    foreach (var g in steamGames)
                    {
                        pbar.Tick($"Currently processing {g.Title}");
                        g.GetLatestSteamNews();
                    }
            }
            RefreshShell();
            
            var adventureGames = games.Where(g => g.Genre.GetGroupName() == Genre.TextAdventure.GetGroupName())
                                      .OrderByDescending(g => g.CountNewsAvailable);

            WriteLine("\nLatest news for all adventure games after news update...\n");
            foreach (var adventureGame in adventureGames)
            {
                adventureGame.DisplayNews();
            }
        }

        protected static Action RefreshShell = () => 
        {
            WriteLine("\nPress ENTER to continue...");
            ReadKey();
            Clear();
        };

    }
}
