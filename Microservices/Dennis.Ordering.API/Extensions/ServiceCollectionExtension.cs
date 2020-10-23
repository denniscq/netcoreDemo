using Dennis.Ordering.API.Application.IntegrationEvents;
using Dennis.Ordering.Domain;
using Dennis.Ordering.Infrastructure;
using Dennis.Ordering.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Extensions
{
    internal static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMediatRService(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OrderContextTransactionBehavior<,>));
            services.AddMediatR(typeof(Order).Assembly, typeof(Program).Assembly);

            return services;
        }

        public static IServiceCollection AddDomainContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddDbContext<OrderContext>(optionsAction);

            return services;
        }

        public static IServiceCollection AddInMemoryDoaminContext(this IServiceCollection services)
        {
            services.AddDomainContext(builder => builder.UseInMemoryDatabase("Dennis"));

            return services;
        }

        public static IServiceCollection AddMysqlDomainContext(this IServiceCollection services, string connectionString)
        {
            services.AddDomainContext(builder => { builder.UseMySql(connectionString); });

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISubscriberService, SubScriberService>();
            services.AddCap(options =>
            {
                // config ef core in order to use transaction to control logic and cap
                options.UseEntityFramework<OrderContext>();

                // config RabbitMQ 
                options.UseRabbitMQ((options) =>
                {
                    configuration.GetSection("RabbitMQ").Bind(options);
                });

                // config Kafuka
                //options.UseKafuka(options => { });
            });

            return services;
        }
    }
}
