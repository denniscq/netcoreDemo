using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dennis.Ordering.API.Extensions
{
    internal static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddMyMiddleware(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<MyMiddleware>();
        }
    }

    internal class MyMiddleware 
    {
        private ILogger<MyMiddleware> _logger;
        private RequestDelegate _requestDelegate;

        public MyMiddleware(ILogger<MyMiddleware> logger, RequestDelegate requestDelegate)
        {
            this._logger = logger;
            this._requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (this._logger.BeginScope("---- TraceIdentifier => {TranceIdentifier} -----", httpContext.TraceIdentifier))
            {
                this._logger.LogInformation("start to inject mymiddleware");
                await _requestDelegate(httpContext);
                this._logger.LogInformation("compeleted");
            }
        }
    }
}
