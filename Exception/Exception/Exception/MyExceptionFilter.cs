using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyException.Exception
{
    public class MyExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var knowException = context.Exception as IKnowException;
            if(knowException == null)
            {
                var ex = context.Exception;
                var logger = context.HttpContext.RequestServices.GetService<ILogger<MyExceptionFilterAttriute>>();
                logger.LogError(ex, ex.Message);

                knowException = KnowException.UnKnowException;
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            else
            {
                knowException = KnowException.FromKnowException(knowException);
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            }

            context.Result = new JsonResult(knowException)
            {
                ContentType = "application/json; charset=utf-8"
            };
        }
    }
}
