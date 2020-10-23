using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Grpc.Services
{
    public class OrderServiceImp : OrderService.OrderServiceBase
    {
        IMediator _mediator;
        ILogger<OrderServiceImp> _logger;

        public OrderServiceImp(IMediator mediator, ILogger<OrderServiceImp> logger)
        {
            this._mediator = mediator;
            this._logger = logger;
        }

        public override async Task<Int64Value> CreateOrder(CreateOrderCommand request, ServerCallContext context)
        {
            this._logger.LogInformation("---- grpc Create Order ----");
            var cmd = new Dennis.Ordering.API.Application.Commands.CreateOrderCommand(request.ItemCount);
            var response = await this._mediator.Send(cmd);
            return new Int64Value { Value = response };
        }

        public override async Task<SearchResponse> Search(SearchRequest request, ServerCallContext context)
        {
            this._logger.LogInformation("---- grpc Search ----");
            var query = new Dennis.Ordering.API.Application.Queries.MyOrderQuery { UserName = request.UserName };
            var result = await this._mediator.Send(query);
            var response = new SearchResponse();
            response.Orders.Add(result);
            return response;
        }
    }
}
