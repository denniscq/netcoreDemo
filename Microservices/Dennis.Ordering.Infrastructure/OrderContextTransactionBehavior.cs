using Dennis.Infrastructure.Core.Behaviors;
using Microsoft.Extensions.Logging;

namespace Dennis.Ordering.Infrastructure
{
    public class OrderContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<OrderContext, TRequest, TResponse>
    {
        public OrderContextTransactionBehavior(ILogger<OrderContextTransactionBehavior<TRequest, TResponse>> logger, OrderContext dbContext) : base(logger, dbContext)
        {
        }
    }
}
