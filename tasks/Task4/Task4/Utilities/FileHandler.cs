using System;
using System.IO;

namespace Task4.Utilities
{
    public interface IFileHandler
    {
        void WriteToFile(string text, string fullFileName);
        string ReadFromFile(string fullFileName);
    }

    public class FileHandler : IFileHandler
    {
        public void WriteToFile(string text, string fullFileName)
        {
            File.WriteAllText(fullFileName, text);
        }

        public string ReadFromFile(string fullFileName)
        {
            if (File.Exists(fullFileName)) return File.ReadAllText(fullFileName);

            //No exception is thrown, because on startup there may be no file available
            Console.WriteLine($"Could not load {fullFileName} because it does not exist.");
            return string.Empty;
        }
        
    }
}
