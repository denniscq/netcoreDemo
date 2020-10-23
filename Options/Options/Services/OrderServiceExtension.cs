using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Options.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrderServiceExtension
    {
        public static IServiceCollection AddMyOrderService(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<OrderServiceOptions>(configuration);
            services.AddOptions<OrderServiceOptions>().Configure(options =>
            {
                configuration.Bind(options);
            })
            #region validate simple

            //.Validate(options =>
            //{
            //    return options.MaxCount < 1000;
            //}, "maxcount can't greater than 1000");

            #endregion

            #region validate annotation

            //.ValidateDataAnnotations();

            #endregion

            #region validate method

            .Services.AddSingleton<IValidateOptions<OrderServiceOptions>, OrderServiceOptionsValidator>();

            #endregion

            services.AddSingleton<IOrderService, OrderService>();

            return services;
        }
    }
}
