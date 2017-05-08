using System;
using System.Collections.Generic;
using System.Linq;
using Task2.Entities;
using Task2.Extensions;

namespace Task2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Run();
        }

        protected static void Run()
        {
            var games = new List<Game>
            {
                new Game(255710 , "Cities Skylines",                Genre.CityBuilder),
                new Game(388210 , "Day of the Tentacle Remastered", Genre.PointAndClick),
                new Game(250740 , "Ragnarok Online",                Genre.Mmorpg),
                new Game(289130 , "Endless Legend",                 Genre.Strategy4X),
                new Game(0      , "Zork",                           Genre.TextAdventure),
                new Game(901660 , "Sam & Max: Season One",          Genre.GraphicAdventure),
                new Game(32340  , "LOOM",                           Genre.PointAndClick)
            };

            foreach (var game in games.OrderBy(g => g.Genre.GetGroupName()))
            {
                Console.WriteLine();
                Console.WriteLine($"{game}");
            }

            Console.WriteLine("Press ENTER to continue...");
            Console.Read();
            Console.Clear();
            
            foreach (var adventure in games.Where(g => g.Genre.GetGroupName() == Genre.TextAdventure.GetGroupName()))
            {
                Console.WriteLine();
                adventure.DisplayNews();
            }
        }

    }
}
