using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dennis.Ordering.API.Application.Commands;
using Dennis.Ordering.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dennis.Ordering.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<long> CreateOrder([FromBody] CreateOrderCommand cmd)
        {
            return await this._mediator.Send(cmd, HttpContext.RequestAborted);
        }

        [HttpGet]
        public async Task<List<string>> QueryOrder([FromQuery] MyOrderQuery myOrderQuery)
        {
            return await this._mediator.Send(myOrderQuery);
        }

        #region traditional implementation

        //[HttpPost]
        //public Task<long> CreateOrder([FromBody] CreateOrderViewModel viewModel)
        //{
        //    //do some logic
        //    var domainModel = viewModel.ToDomainModel();
        //    return await orderService.CreateOrder(model);
        //}

        //class OrderService : IOrderService
        //{
        //    public long CreateOrder(CreateOrderModel model)
        //    {
        //        var address = new Address();
        //        var order = new Order();

        //        _orderRepository.Add(order);
        //        return order.Id;
        //    }
        //}

        #endregion
    }
}