using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Dennis.Mobile.Gateway.Controllers
{
    [Route("account/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public async Task<string> Login()
        {
            return await Task.FromResult("请先登陆!!!");
        }

        public async Task<IActionResult> CookieLogin([FromQuery]string name)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("Name", name));
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return this.Content("login successfully");
        }

        public async Task<IActionResult> JwtLogin([FromServices] SymmetricSecurityKey securityKey, [FromQuery] string name)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("name", name));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //var token = new JwtSecurityToken(issuer: "localhost", audience: "localhost", claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: credential);

            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credential);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return this.Content(tokenString);
        }
    }
}