using Dennis.Domain.Abstractions;
using System;

namespace Dennis.Ordering.Domain
{
    public class Order : Entity<long>, IAggregateRoot
    {
        public Order()
        {
        }

        public Order(string userId, string userName, int itemCound, Address address)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.ItemCound = itemCound;
            this.Address = address;

            this.AddDomainEvent(new OrderCreatedDomainEvent(this));
        }

        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public int ItemCound { get; private set; }
        public Address Address { get; private set; }

        public void ChangeAddress(Address address)
        {
            this.Address = address;
            this.AddDomainEvent(new OrderAddressChangedDomainEvent(this));
        }
    }
}
