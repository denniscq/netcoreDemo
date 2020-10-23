using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Routing.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        [HttpGet("{id:isLong}")]
        public bool OrderExist([FromRoute]string id)
        {
            return true;
        }

        [HttpGet("{id:max(20)}")]
        public bool Max([FromRoute]long id, [FromServices]LinkGenerator linkGenerator)
        {
            var a = linkGenerator.GetPathByAction(HttpContext, action: "Reque", controller: "Order", values: new { name = "abc" });
            var b = linkGenerator.GetUriByAction(HttpContext, action: "Reque", controller: "Order", values: new { name = "abc" });

            return true;
        }

        [HttpGet("{name:required}")]
        [Obsolete]
        public bool Reque(string name)
        {
            return true;
        }

        [HttpGet("{number:regex(^\\d{{3}}$)}")]
        public bool Number(string number)
        {
            return true;
        }
    }
}