using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task4.Entities;
using Task4.Extensions;
using Task4.Utilities;

namespace Task4.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();
        void Save(IEnumerable<Game> games);
    }

    public class GameRepository : IGameRepository
    {
        private static string FullFileName => Path.Combine(Environment.CurrentDirectory, "games.json");
        
        private readonly IFileHandler _fileHandler;
        
        public GameRepository(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }
        
        public void Save(IEnumerable<Game> games)
        {
            var json = JsonConvert.SerializeObject(games);

            _fileHandler.WriteToFile(json, FullFileName);
        }

        public IEnumerable<Game> GetAllGames()
        {
            var json = _fileHandler.ReadFromFile(FullFileName);
            
            if (!string.IsNullOrWhiteSpace(json) && !json.IsValidJson())
            {
                throw new FormatException($"{FullFileName} is not a valid json.");
            }
            
            try
            {
                return JsonConvert.DeserializeObject<IEnumerable<Game>>(json) ?? Enumerable.Empty<Game>();
            }
            catch (JsonSerializationException)
            {
                Console.WriteLine("Whoooops");
            }

            return Enumerable.Empty<Game>();
        }

    }
}
