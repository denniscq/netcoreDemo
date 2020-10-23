using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyException.Exception
{
    public class MyExceptionFilterAttriute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            IKnowException knowException = context.Exception as IKnowException;
            if(knowException == null)
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<MyExceptionFilterAttriute>>();
                logger.LogError(context.Exception, context.Exception.Message + "chenqiang");

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
                ContentType = "application/json; charset=uft-8"
            };
        }
    }
}
