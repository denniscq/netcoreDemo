﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Dennis.Ordering.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HcController : ControllerBase
    {
        [HttpGet]
        public IActionResult SetReady([FromQuery]bool ready)
        {
            Startup.IsReady = ready;
            if (!ready)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(30000);
                    Startup.IsReady = true;
                });
            }

            return this.Content($"{Environment.MachineName} : Ready={Startup.IsReady}");
        }

        [HttpGet]
        public IActionResult SetLive([FromQuery]bool live)
        {
            Startup.IsLive = live;

            return this.Content($"{Environment.MachineName} : Live={Startup.IsLive}");
        }

        [HttpGet]
        public IActionResult Exit([FromServices]IHostApplicationLifetime application)
        {
            Task.Run(async () =>
            {
                await Task.Delay(3000);
                application.StopApplication();
            });

            return this.Content($"{Environment.MachineName} => Stopping");
        }
    }
}