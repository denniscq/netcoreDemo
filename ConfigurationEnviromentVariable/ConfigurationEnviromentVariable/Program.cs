using Microsoft.Extensions.Configuration;
using System;

namespace ConfigurationEnviromentVariable
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //configurationBuilder.AddEnvironmentVariables();
            //var configurationRoot = configurationBuilder.Build();

            //Console.WriteLine($"key1 => {configurationRoot["key1"]}");
            //Console.WriteLine($"key2 => {configurationRoot["key2"]}");
            //Console.WriteLine($"section1 => {configurationRoot.GetSection("section1").Value}");
            //Console.WriteLine($"section2 => key1 => {configurationRoot.GetSection("section2")["key1"]}");
            //Console.WriteLine($"section2 => unexsited key2 => {configurationRoot.GetSection("section2")["key2"]}");

            configurationBuilder.AddEnvironmentVariables("dennis_");
            var configurationRoot = configurationBuilder.Build();
            Console.WriteLine($"dennis => key1 => {configurationRoot["key1"]}");

            Console.ReadKey();
        }
    }
}
