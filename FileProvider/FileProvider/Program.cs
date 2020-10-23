using Microsoft.Extensions.FileProviders;
using System;

namespace FileProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            IFileProvider pysicalProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory);
            //var contents = pysicalProvider.GetDirectoryContents("/");
            //foreach (var item in contents)
            //{
            //    Console.WriteLine(item.Name);
            //}


            IFileProvider embeddedProvider = new EmbeddedFileProvider(typeof(Program).Assembly);
            var html = embeddedProvider.GetFileInfo("test.html");

            var compositeProvider = new CompositeFileProvider(pysicalProvider, embeddedProvider);
            var allContents = compositeProvider.GetDirectoryContents("/");
            foreach (var item in allContents)
            {
                Console.WriteLine(item.Name);
            }


            Console.ReadKey();
        }
    }
}
