using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logging
{
    public class OrderService
    {
        ILogger<OrderService> logger;

        public OrderService(ILogger<OrderService> logger)
        {
            this.logger = logger;
        }

        public void Show()
        {
            this.logger.LogInformation("Show Time {time}", DateTime.Now);

            this.logger.LogInformation($"Show Time {DateTime.Now}");
        }
    }
}
