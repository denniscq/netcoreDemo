using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dennis.Ordering.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Abc()
        {
            return this.Content("this is order service");
        }

        [HttpGet]
        public IActionResult Error()
        {
            throw new Exception("test eror");
        }
    }
}