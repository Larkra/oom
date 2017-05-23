using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4.Entities;
using Task4.Extensions;

namespace Task4.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetEntities();
        T GetEntityById(long id);

        void SaveEntity(T entity);
        void UpdateEntity(T entity);
    }

    public class Repository<T> where T : class, IRepository<T>
    {
        private readonly string _path = Environment.CurrentDirectory;

        private IEnumerable<T> _entities;

        public IEnumerable<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = GetEntities();
                }
                return _entities;
            }
        }

        private string GetFullFilename(string filename)
        {
            return Path.Combine(_path, filename);
        }

        IEnumerable<T> GetEntities()
        {
            var filename = "games.json";

            var fullFilename = GetFullFilename(filename);

            if (!File.Exists(fullFilename))
            {
                Console.WriteLine($"Can not load {filename} because it does not exist.");
                return string.Empty;
            }

            var json = File.ReadAllText(fullFilename);
            if (!json.IsValidJson())
            {
                Console.WriteLine($"{nameof(LoadFromJsonFile)} can not load {filename} because the given string is not a valid json.");
                return string.Empty;
            }

            return json;
        }

        T GetEntityById(long id)
        {
            throw new NotImplementedException();
        }

        void SaveEntity(T entity)
        {
            throw new NotImplementedException();
        }

        void UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
