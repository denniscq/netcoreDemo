using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;

namespace ConfigurationFile
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsetting.json", optional: false, reloadOnChange: true);
            configurationBuilder.AddJsonFile("appsetting.Development.json", optional: true);
            var configurationRoot = configurationBuilder.Build();

            //IChangeToken changeToken = configurationRoot.GetReloadToken();

            //ChangeToken.OnChange(() => configurationRoot.GetReloadToken(), () =>
            //{
            //    Console.WriteLine($"key1 => {configurationRoot["key1"]}");
            //    Console.WriteLine($"key2 => {configurationRoot["key2"]}");

            //    var section3 = configurationRoot.GetSection("section3");
            //    Console.WriteLine($"section3 => key3 => {section3["key3"]}");
            //});

            //changeToken.RegisterChangeCallback((state) =>
            //{
            //    Console.WriteLine($"key1 => {configurationRoot["key1"]}");
            //    Console.WriteLine($"key2 => {configurationRoot["key2"]}");

            //    var section3 = configurationRoot.GetSection("section3");
            //    Console.WriteLine($"section3 => key3 => {section3["key3"]}");

            //    changeToken = configurationRoot.GetReloadToken();
            //}, configurationRoot);

            //Console.WriteLine($"key1 => {configurationRoot["key1"]}");
            //Console.WriteLine($"key2 => {configurationRoot["key2"]}");

            //var section3 = configurationRoot.GetSection("section3");
            //Console.WriteLine($"section3 => key3 => {section3["key3"]}");

            //Console.ReadKey();

            Console.WriteLine($"key1 => {configurationRoot["key1"]}");
            Console.WriteLine($"key2 => {configurationRoot["key2"]}");

            var config = new Config
            {
                key1 = "chen"
            };
            configurationRoot.Bind(config, options => { options.BindNonPublicProperties = true; });

            Console.WriteLine($"binder => key1 => {config.key1}");
            Console.WriteLine($"binder => key2 => {config.key2}");

            Console.ReadKey();
        }
    }

    class Config
    {
        public string key1 { get; set; }
        public string key2 { get; private set; }
    }
}
