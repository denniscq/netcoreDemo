using Dennis.Ordering.Domain;
using Dennis.Ordering.Infrastructure;
using Dennis.Ordering.Infrastructure.Repositories;
using DotNetCore.CAP;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        IOrderRepository _orderRepository;
        ICapPublisher _capPublisher;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, ICapPublisher capPublisher)
        {
            this._orderRepository = orderRepository;
            this._capPublisher = capPublisher;
        }

        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var address = new Address("zhongshan", "dalian", "001");
            var order = new Order("11510417", "qiang.c.chen", (int)request.ItemCount, address);
            await this._orderRepository.AddAsync(order);
            await this._orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return order.Id;
        }
    }
}
