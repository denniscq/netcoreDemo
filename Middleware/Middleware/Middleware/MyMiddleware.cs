using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Middleware
{
    public class MyMiddleware
    {
        RequestDelegate _next;
        ILogger _logger;

        public MyMiddleware(RequestDelegate next, ILogger<MyMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (_logger.BeginScope("TraceIdentifier:{TraceIdentifier}", context.TraceIdentifier))
            {
                _logger.LogInformation("start execute");

                await _next(context);

                _logger.LogInformation("end execute");
            }
        }
    }
}
