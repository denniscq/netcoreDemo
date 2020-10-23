using Grpc.Core;
using GrpcServices;
using System;
using System.Threading.Tasks;

namespace grpcServer.GrpcService
{
    public class OrderService : OrderGrpc.OrderGrpcBase
    {
        public override Task<CreateOrderResult> CreateOrder(CreateOrderCommand request, ServerCallContext context)
        {
            //throw new Exception("order server error");
            return Task.FromResult(new CreateOrderResult { OrderId = 10 });
        }
    }
}
