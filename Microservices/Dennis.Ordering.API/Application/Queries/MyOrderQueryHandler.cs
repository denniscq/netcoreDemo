using Dennis.Ordering.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Application.Queries
{
    public class MyOrderQueryHandler : IRequestHandler<MyOrderQuery, List<string>>
    {
        IOrderRepository _orderRepository;

        public MyOrderQueryHandler(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public Task<List<string>> Handle(MyOrderQuery request, CancellationToken cancellationToken)
        {
            return this._orderRepository.GetCitiesByName(request.UserName);
            //return Task.FromResult(new List<string> { DateTime.Now.ToString() });

        }
    }
}
