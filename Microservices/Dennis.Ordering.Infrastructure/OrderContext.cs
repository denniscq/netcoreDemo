using Dennis.Infrastructure.Core;
using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Dennis.Ordering.Infrastructure
{
    public class OrderContext : EFContext
    {
        public OrderContext(DbContextOptions options, IMediator mediator, ICapPublisher capBus) : base(options, mediator, capBus)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
