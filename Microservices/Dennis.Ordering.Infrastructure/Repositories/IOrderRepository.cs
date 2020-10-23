using Dennis.Infrastructure.Core;
using Dennis.Ordering.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dennis.Ordering.Infrastructure.Repositories
{
    public interface IOrderRepository : IRepository<Order, long>
    {
        Task<List<string>> GetCitiesByName(string userName);
    }
}
