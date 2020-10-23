using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace CustomConfiguration
{
    class MyConfigurationProvider : ConfigurationProvider
    {
        public Timer timer;

        public MyConfigurationProvider()
        {
            Console.WriteLine("==== init provider ====");

            this.timer = new Timer();
            this.timer.Elapsed += ElapsedEventHandler;
            this.timer.Interval = 3000;
            this.timer.Start();
        }

        public override void Load()
        {
            this.Load(false);
        }

        private void ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            this.Load(true);
        }

        private void Load(bool isReload)
        {
            this.Data["lastTime"] = DateTime.Now.ToString();
            if (isReload)
            {
                base.OnReload();
            }
        }
    }
}
