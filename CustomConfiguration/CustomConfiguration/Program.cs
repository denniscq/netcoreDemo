using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;

namespace CustomConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddCustomConfiguration();

            var configurationRoot = configurationBuilder.Build();
            ChangeToken.OnChange(() => configurationRoot.GetReloadToken(), () =>
            {
                Console.WriteLine($"lastTime => {configurationRoot["lastTime"]}");
            });

            Console.WriteLine("==== start ====");

            Console.ReadKey();
        }
    }
}
