﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dennis.Mobile.Gateway.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public IActionResult Abc()
        {
            return Content("this is gateway");
        }

        public IActionResult ShowConfig([FromServices]IConfiguration configuration)
        {
            return Content(configuration["ENV_ABC"]);
        }
    }
}