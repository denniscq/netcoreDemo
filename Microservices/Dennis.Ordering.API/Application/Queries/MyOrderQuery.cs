using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Application.Queries
{
    public class MyOrderQuery : IRequest<List<string>>
    {
        public string UserName { get; set; }
    }
}
