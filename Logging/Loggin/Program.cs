using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;

namespace Logging
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = configurationBuilder.Build();

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(p => config);

            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(config.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            #region Basic Logging

            //serviceCollection.AddTransient<OrderService>();

            //var order = serviceProvider.GetService<OrderService>();
            //order.Show();

            //ILoggerFactory loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            //ILogger testLogger = loggerFactory.CreateLogger("testLogger");
            //testLogger.LogDebug(2000, "test1");
            //testLogger.LogInformation("test2");
            //testLogger.LogError(new Exception("test error"), "chenqiang");

            //ILogger testLogger1 = loggerFactory.CreateLogger("testLogger");
            //testLogger1.LogDebug("aiya");

            //Console.WriteLine(testLogger == testLogger1);

            #endregion

            #region Scope Logging

            var testLogger3 = serviceProvider.GetService<ILogger<Program>>();
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                using (testLogger3.BeginScope("ScopeId:{scopeId}", Guid.NewGuid()))
                {
                    testLogger3.LogInformation("this is info");
                    testLogger3.LogDebug("this is debug");
                    testLogger3.LogTrace("this is trace");
                }
            }

            Console.WriteLine("=================== split line =====================");

            #endregion

            Console.ReadKey();
        }
    }
}
