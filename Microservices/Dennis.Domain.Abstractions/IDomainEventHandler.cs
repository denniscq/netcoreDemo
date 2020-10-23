using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dennis.Domain.Abstractions
{
    public interface IDomainEventHandler<TDomainEvent>: INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent
    {
    }
}
