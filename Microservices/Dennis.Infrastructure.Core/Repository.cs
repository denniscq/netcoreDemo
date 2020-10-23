using Dennis.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dennis.Infrastructure.Core
{
    public abstract class Repository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot where TDbContext : EFContext
    {
        protected TDbContext DbContext;

        public Repository(TDbContext context)
        {
            this.DbContext = context;
        }

        public IUnitOfWork UnitOfWork => this.DbContext;

        public virtual TEntity Add(TEntity entity)
        {
            return this.DbContext.Add(entity).Entity;
        }

        public virtual Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.Add(entity));
        }

        public virtual bool Remove(Entity entity)
        {
            var result = this.DbContext.Remove(entity);
            return true;
        }

        public Task<bool> RemoveAsync(TEntity entity)
        {
            return Task.FromResult(this.Remove(entity));
        }

        public TEntity Update(TEntity entity)
        {
            return this.DbContext.Update(entity).Entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.Update(entity));
        }
    }

    public abstract class Repository<TEntity, TKey, TDbContext> : Repository<TEntity, TDbContext>, IRepository<TEntity, TKey> where TEntity: Entity<TKey>, IAggregateRoot where TDbContext : EFContext
    {
        public Repository(TDbContext context) : base(context)
        {
        }

        public bool Delete(TKey id)
        {
            var entity = this.DbContext.Find<TEntity>(id);
            if(entity != null)
            {
                this.DbContext.Remove(entity);
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await this.DbContext.FindAsync<TEntity>(id);
            if(entity != null)
            {
                this.DbContext.Remove(entity);
                return true;
            }

            return false;
        }

        public TEntity Get(TKey id)
        {
            return this.DbContext.Find<TEntity>(id);
        }

        public async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await this.DbContext.FindAsync<TEntity>(id, cancellationToken);
        }
    }
}
