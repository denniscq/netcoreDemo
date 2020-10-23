using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyException.Exception;

namespace MyException.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("/error")]
        public IActionResult Index()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = exceptionHandlerPathFeature?.Error;

            var knowException = ex as IKnowException;
            if(knowException == null)
            {
                var logger = HttpContext.RequestServices.GetService<ILogger<MyExceptionFilterAttriute>>();
                logger.LogError(ex, ex.Message);

                knowException = KnowException.UnKnowException;
            }
            else
            {
                knowException = KnowException.FromKnowException(knowException);
            }

            return View(knowException);
        }
    }
}