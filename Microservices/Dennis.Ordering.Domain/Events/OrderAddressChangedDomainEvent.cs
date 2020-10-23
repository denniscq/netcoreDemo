using Dennis.Domain.Abstractions;

namespace Dennis.Ordering.Domain
{
    internal class OrderAddressChangedDomainEvent : IDomainEvent
    {
        private Order order;

        public OrderAddressChangedDomainEvent(Order order)
        {
            this.order = order;
        }
    }
}