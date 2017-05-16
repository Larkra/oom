using System.Linq;
using Task3.Entities;
using Task3.Enums;
using Task3.Interfaces;
using static System.Console;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("... Starting total meaningless program...");
            Run();
        }

        private static void Run()
        {
            var games = new IGame[]
            {
                new VideoGame("Monkey Island", Genre.PointAndClick, Platform.PC),
                new VideoGame("Cities Skylines", Genre.CityBuilder, Platform.PS4|Platform.XBoxOne),
                new TabletopGame("Chess", Genre.BoardGame)
            };

            foreach (var g in games)
            {
                WriteLine();
                WriteLine($"{g.GetDescription()}");
            }

            var videoGames = games.Where(g => g.GetType() == typeof(VideoGame)).ToList();
            foreach (var game in videoGames)
            {
                var videoGame = (VideoGame) game;
                videoGame.RaisePlayThroughs();
            }

            foreach (var g in games)
            {
                WriteLine();
                WriteLine($"{g.GetDescription()}");
            }
        }
        
    }
}
