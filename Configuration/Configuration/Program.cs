using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Configuration
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "section3:key3", "value3" },
                { "section4:key4", "value4" },
                { "section4:section5:key5", "value5" }
            });
            var configurationRoot = configurationBuilder.Build();

            Console.WriteLine($"Key1 => {configurationRoot["key1"]}");
            Console.WriteLine($"Key2 => {configurationRoot["key2"]}");
            Console.WriteLine($"not existed Key3 => {configurationRoot["key3"]}");

            var section3 = configurationRoot.GetSection("section3");
            Console.WriteLine($"Section3 => Key3 => {section3["key3"]}");

            var section4 = configurationRoot.GetSection("section4");
            Console.WriteLine($"Section4 => Key4 => {section4["key4"]}");

            var section5 = section4.GetSection("section5");
            Console.WriteLine($"Section5 => Key5 => {section5["key5"]}");

            Console.WriteLine($"not existed Section5 => Key6 => {section5["key6"]}");

            Console.WriteLine("output successfully");
            Console.ReadKey();
        }
    }
}
