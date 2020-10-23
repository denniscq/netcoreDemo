using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dennis.Mobile.ApiAggregator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dennis.Mobile.ApiAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderService _orderService;
        Ordering.API.Grpc.OrderService.OrderServiceClient _grpcClient;

        public OrderController(IOrderService orderService, Ordering.API.Grpc.OrderService.OrderServiceClient grpcClient)
        {
            this._orderService = orderService;
            this._grpcClient = grpcClient;
        }

        [HttpGet]
        public ActionResult GetOrders([FromQuery] Ordering.API.Grpc.SearchRequest searchRequest)
        {
            var result = this._grpcClient.Search(searchRequest);
            return this.Ok(result.Orders);
        }
    }
}