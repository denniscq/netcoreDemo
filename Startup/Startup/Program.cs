using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                Console.WriteLine("ConfigureWebHostDefaults");
                webBuilder.UseStartup<Startup>();

                webBuilder.ConfigureServices(services => {
                    Console.WriteLine("StartUp.ConfigureServices1");
                });

                webBuilder.Configure(p =>
                {
                    Console.WriteLine("Startup.Configure1");
                });
            })
            .ConfigureServices(service =>
            {
                Console.WriteLine("ConfigureServices");
            })
            .ConfigureAppConfiguration(builder =>
            {
                Console.WriteLine("ConfigureAppConfiguration");
            })
            .ConfigureHostConfiguration(builder =>
            {
                Console.WriteLine("ConfigureHostConfiguration");
            });
    }
}
