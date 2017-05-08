using System;
using System.Configuration;

namespace Task1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var p = new Program();
            
            p.Run();
        }

        public void Run()
        {
            var text = GetConfiguredOutputText();

            Console.WriteLine($"{text}");
        }

        private static string GetConfiguredOutputText() => string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["OutputText"])
            ? "So Long, and Thanks for All the Fish"
            : ConfigurationManager.AppSettings["OutputText"];
    }
}