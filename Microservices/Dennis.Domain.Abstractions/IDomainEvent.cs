using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dennis.Domain.Abstractions
{
    public interface IDomainEvent: INotification
    {
    }
}
