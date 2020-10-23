using Dennis.Infrastructure.Core.Extensions;
using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dennis.Infrastructure.Core
{
    public class EFContext : DbContext, IUnitOfWork, ITransaction
    {
        protected IMediator _mediator;
        ICapPublisher _capBus;
        private IDbContextTransaction _currentTransaction;

        public EFContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus) : base(options)
        {
            this._mediator = mediator;
            this._capBus = capBus;
        }

        public bool HasActiveTransaction => this._currentTransaction != null;

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (this._currentTransaction != null) return null;

            this._currentTransaction = Database.BeginTransaction(this._capBus, autoCommit: false);
            return Task.FromResult(this._currentTransaction);
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != this._currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is no current one.");

            try
            {
                await this.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception dbex)
            {
                this.RollbackTransaction();
                throw;
            }
            finally
            {
                if(this._currentTransaction != null)
                {
                    this._currentTransaction.Dispose();
                    this._currentTransaction = null;
                }
            }
        }

        public IDbContextTransaction GetCurrentTransaction() => this._currentTransaction;

        public void RollbackTransaction()
        {
            try
            {
                this._currentTransaction?.Rollback();
            }
            finally
            {
                if(this._currentTransaction != null)
                {
                    this._currentTransaction.Dispose();
                    this._currentTransaction = null;
                }
            }
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            await this._mediator.DispatchDomainEventsAsync(this);

            return true;
        }
    }
}
