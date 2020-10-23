using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ConfigurationCommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //configurationBuilder.AddCommandLine(args);

            #region Command Replace

            var mapper = new Dictionary<string, string>()
            {
                { "--k1", "CommandLineKey2" }
            };

            configurationBuilder.AddCommandLine(args, mapper);

            #endregion

            var configurationRoot = configurationBuilder.Build();


            Console.WriteLine($"CommandLineKey1 => {configurationRoot["CommandLineKey1"]}");
            Console.WriteLine($"CommandLineKey2 => {configurationRoot["CommandLineKey2"]}");
            Console.WriteLine($"CommandLineKey3 => {configurationRoot["CommandLineKey3"]}");

            Console.ReadKey();
        }
    }
}
