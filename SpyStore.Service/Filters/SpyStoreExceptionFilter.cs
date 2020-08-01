using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpyStore.Dal.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.Exceptions;
using Microsoft.Extensions.Hosting;

namespace SpyStore.Service.Filters
{
    public class SpyStoreExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _hostEnviroment;
        public SpyStoreExceptionFilter(IHostEnvironment hostenvironment)
        {
            _hostEnviroment = hostenvironment;
        }
        public override void OnException(ExceptionContext context)
        {
            bool isDevelopment = _hostEnviroment.IsDevelopment();
            var ex = context.Exception;
            string stackTrace =
            (isDevelopment) ? context.Exception.StackTrace : string.Empty;
            string message = ex.Message;
            string error = string.Empty;
            IActionResult actionResult;
            switch (ex)
            {
                case SpyStoreInvalidQuantityException iqe:
                    //returns a 400
                    error= "Invalid quantity request.";
                    actionResult = new BadRequestObjectResult(new
                    { Error = error, Message = message, stackTrace = stackTrace });
                    break;
                case DbUpdateConcurrencyException ce:
                //Returns a 400
                error= "Concurency Issue";
                    actionResult = new BadRequestObjectResult(new
                    {
                        Error = error,
                        Message = message,
                        StackTrace = stackTrace
                    });
                    break;
                case SpyStoreInvalidProductException ipe:
                    error = "Invalid Product Id. ";
                    actionResult = new BadRequestObjectResult(new
                    {
                        Error = error,
                        Message = message,
                        StackTrace = stackTrace
                    });
                    break;
                case SpyStoreInvalidCustomerException ice:
                    error = "Invalid Customer Id";
                    actionResult = new BadRequestObjectResult(new { Error = error, Message = message, StackTrace = stackTrace });
                    break;
                default:
                    error = "General Error";
                    actionResult = new ObjectResult(new
                    {
                        Error = error,
                        Message = message,
                        StackTrace = stackTrace
                    })
                    { StatusCode = 500 };
                    break;
            }
            // context.ExceptionHandled = true;
            context.Result = actionResult;
        }
       
       
    }
    

}
