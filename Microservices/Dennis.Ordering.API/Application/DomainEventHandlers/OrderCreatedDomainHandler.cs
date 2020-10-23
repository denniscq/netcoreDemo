using Dennis.Domain.Abstractions;
using Dennis.Ordering.API.Application.IntegrationEvents;
using Dennis.Ordering.Domain;
using DotNetCore.CAP;
using System.Threading;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Application.DomainEventHandlers
{
    public class OrderCreatedDomainHandler : IDomainEventHandler<OrderCreatedDomainEvent>
    {
        ICapPublisher capPublisher;

        public OrderCreatedDomainHandler(ICapPublisher capPublisher)
        {
            this.capPublisher = capPublisher;
        }

        public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await this.capPublisher.PublishAsync("OrderCreated", new { id = notification.Order.Id });
        }
    }
}
