using Dennis.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dennis.Infrastructure.Core.Extensions
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext dbContext)
        {
            var domainEntities = dbContext.ChangeTracker.Entries<Entity>().Where(p => p.Entity.DomainEvents != null && p.Entity.DomainEvents.Any());
            var domainEvents = domainEntities.SelectMany(p => p.Entity.DomainEvents);

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
