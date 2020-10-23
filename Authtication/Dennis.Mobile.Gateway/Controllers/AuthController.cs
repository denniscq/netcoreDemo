using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dennis.Mobile.Gateway.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : Controller
    {
        [Authorize]
        public IActionResult Cookie()
        {
            return this.Content("operate successfully with cookie");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Jwt()
        {
            return this.Content($"operate successfully with jwt, the user is {User.FindFirst("name").Value}");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Anyone()
        {
            return this.Content($"operate successfully, the user is {User.FindFirst("name").Value}");
        }
    }
}