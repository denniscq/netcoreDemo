using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Options.Services
{
    public interface IOrderService 
    {
        int ShowMaxCount();
    }

    public class OrderService : IOrderService
    {
        //IOptions<OrderServiceOptions> options;
        //IOptionsSnapshot<OrderServiceOptions> options;
        IOptionsMonitor<OrderServiceOptions> options;
        public OrderService(IOptionsMonitor<OrderServiceOptions> options)
        {
            this.options = options;

            options.OnChange((option, param2) =>
            {
                Console.WriteLine(param2);
                Console.WriteLine($"Changed value => {option.MaxCount}");
            });
        }

        public int ShowMaxCount()
        {
            //return this.options.Value.MaxCount;
            return this.options.CurrentValue.MaxCount;
        }
    }

    public class OrderServiceOptions
    {
        [Range(1, 20)]
        public int MaxCount { get; set; } = 100;
    }

    public class OrderServiceOptionsValidator : IValidateOptions<OrderServiceOptions>
    {
        public ValidateOptionsResult Validate(string name, OrderServiceOptions options)
        {
            if(options.MaxCount < 1000)
            {
                return ValidateOptionsResult.Success;
            }

            return ValidateOptionsResult.Fail("max count can't greater than 1000");
        }
    }
}
