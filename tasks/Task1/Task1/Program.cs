using System;
using System.Configuration;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();      
        }

        private void Run()
        {
            Console.WriteLine($"{ConfiguredOutputText}");

            return;
        }

        private string ConfiguredOutputText
        {
            get
            {
                var outputText = "default";

                if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["output"]))
                {
                    outputText = ConfigurationManager.AppSettings["output"].ToString();
                }

                return outputText;
            }
        }

    }
}