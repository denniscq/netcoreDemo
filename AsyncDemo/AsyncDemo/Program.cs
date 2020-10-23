
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            var aa = new Program();
            var bb = aa.layer1(aa);
            Console.WriteLine("welcome");
            Console.ReadKey();
        }



        public async Task<int> layer1(Program aa)
        {
            Console.WriteLine("layer1 first");
            Console.WriteLine("1"+Thread.CurrentThread.ManagedThreadId);
            //await Task.Delay(5000);
            await aa.laylay();
            Console.WriteLine("2"+Thread.CurrentThread.ManagedThreadId);
            //aa.layer2_1();
            //aa.layer2_2();
            //aa.layer2_3();
            Console.WriteLine("layer1 last");
            return 1;
        }

        public async Task<int> laylay()
        {
            HttpClient httpClient = new HttpClient();
            var aa = await httpClient.GetAsync("https://localhost:5001/weatherforecast");
            Console.WriteLine("3"+Thread.CurrentThread.ManagedThreadId);
            return 1;
        }



        public async Task layer2()
        {
            Console.WriteLine("layer2 first");
            //await Task.Run(() =>
            //{
            //    Thread.Sleep(1000 * 10);
            //    Console.WriteLine("layer2");
            //});
            await this.layer3();
            Console.WriteLine("layer2 last");
        }

        public async Task layer3()
        {
            Console.WriteLine("111");
            Thread.Sleep(1000);
            Console.WriteLine("123");
        }



        public void layer2_1()
        {
            Console.WriteLine("layer2_1");
        }



        public void layer2_2()
        {
            var bb = new List<string> { "1", "2", "3", "4", "5", "6" };
            bb.ForEach(p =>
            {
                Console.WriteLine(p);
            });
        }



        public async void layer2_3()
        {
            var bb = new List<int> { 1, 2, 3, 4, 5 };



            bb.ForEach(async p =>
            {
                await Task.Run(() =>
                {
                    Thread.Sleep(p * 1000 / p);
                    Console.WriteLine(p);
                });



                // do something
            });



            //await bb.ForEach1<int>(async p =>
            //{
            //    Thread.Sleep(p * 1000 / p);
            //    Console.WriteLine(p);
            //});
            Console.WriteLine("layer2_3");
            await this.test();
        }

        public Task test()
        {
            return Task.CompletedTask;
        }
    }
}
