using Dennis.Infrastructure.Core.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dennis.Infrastructure.Core.Behaviors
{
    public class TransactionBehavior<TDbContext, TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TDbContext : EFContext
    {
        private ILogger _logger;
        private TDbContext _dbContext;

        public TransactionBehavior(ILogger logger, TDbContext dbContext)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (this._dbContext.HasActiveTransaction)
                {
                    return await next();
                }

                var strategy = this._dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using(var transaction = await this._dbContext.BeginTransactionAsync())
                    using (this._logger.BeginScope("TransactonContext:{transactionId}", transaction.TransactionId))
                    {
                        this._logger.LogInformation("---- start transaction {TrasactionId} {CommandName} ({@Command})", transaction.TransactionId, typeName, request);
                        response = await next();
                        this._logger.LogInformation("---- commit transaction {TransactionId} {CommandName}", transaction.TransactionId, typeName);

                        await this._dbContext.CommitTransactionAsync(transaction);
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "an error has been occoured while handing transaction {CommandName} ({@Command})", typeName, request);
                throw;
            }
        }
    }
}
