using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Application.Commands
{
    public class CreateOrderCommand : IRequest<long>
    {
        public CreateOrderCommand(int itemCount)
        {
            this.ItemCount = itemCount;
        }

        public long ItemCount { get; private set; }
    }
}
