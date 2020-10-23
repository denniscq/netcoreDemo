using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpClientFactory.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HttpClientFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get([FromServices] OrderServiceClient orderServiceClient)
        {
            return await orderServiceClient.Get();
        }

        [HttpGet("getbynamed")]
        public async Task<string> GetByNamed([FromServices] NamedOrderService namedOrderService)
        {
            return await namedOrderService.Get();
        } 
        
        [HttpGet("getbytype")]
        public async Task<string> GetByType([FromServices] TypedOrderService typedOrderService)
        {
            return await typedOrderService.Get();
        }
    }
}