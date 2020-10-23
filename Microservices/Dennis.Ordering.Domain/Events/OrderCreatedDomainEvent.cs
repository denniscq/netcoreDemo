using Dennis.Domain.Abstractions;

namespace Dennis.Ordering.Domain
{
    public class OrderCreatedDomainEvent : IDomainEvent
    {
        public Order Order { get; private set; }

        public OrderCreatedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}