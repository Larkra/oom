using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Task3.Entities;
using Task3.Enums;
using Task3.Interfaces;
using static System.Console;

namespace Task3
{
    class Program
    {
        #region Mocks
        private static IEnumerable<IGame> GameMocks => new List<IGame>
        {
            new VideoGame("Monkey Island", Genre.PointAndClick, Platform.PC),
            new VideoGame("Bioshock Infinite", Genre.Shooter, Platform.PC|Platform.PS4|Platform.XBoxOne),
            new VideoGame("Colonization", Genre.Strategy4X, Platform.PC),
            new TabletopGame("Chess", Genre.BoardGame),
            new TabletopGame("Solitaire", Genre.CardGame)
        };
        #endregion

        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            var games = GameMocks.ToList();

            DisplayGames(games, $"Before calling {nameof(VideoGame.RaisePlayThroughs)}...");
            
            games.ForEach(g =>
            {
                var vg = g as VideoGame;
                vg?.RaisePlayThroughs();
            });
            
            DisplayGames(games, $"After calling {nameof(VideoGame.RaisePlayThroughs)}...");
            
            DoJsonStuff(games);
        }

        private static void DoJsonStuff(IEnumerable<IGame> games)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };

            var jsonToWrite = JsonConvert.SerializeObject(games, settings);

            var fullFileName = Path.Combine(Environment.ExpandEnvironmentVariables("%temp%"), "games.json");
            File.WriteAllText(fullFileName, jsonToWrite);

            var jsonRead = File.ReadAllText(fullFileName);
            var extractedGames = JsonConvert.DeserializeObject<IList<IGame>>(jsonRead, settings);

            DisplayGames(extractedGames, "After deserializing...");
        }

        private static void DisplayGames(IEnumerable<IGame> games, string msg)
        {
            WriteLine(msg);
            games.ToList().ForEach(g =>
            {
                WriteLine();
                WriteLine(g.GetDescription());
            });
            RefreshShell();
        }

        private static readonly Action RefreshShell = () =>
        {
            WriteLine();
            WriteLine("Press any key to continue...");
            ReadKey();
            Clear();
        };

    }
}
