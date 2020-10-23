using Dennis.Infrastructure.Core;
using Dennis.Ordering.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dennis.Ordering.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order, long, OrderContext>, IOrderRepository
    {
        public OrderRepository(OrderContext context) : base(context)
        {
        }

        public Task<List<string>> GetCitiesByName(string userName)
        {
            var cities = (from order in this.DbContext.Set<Order>()
                          where order.UserName == userName && order.Address != null
                          select order.Address.City).ToList();
            return Task.FromResult(cities);
        }
    }
}


