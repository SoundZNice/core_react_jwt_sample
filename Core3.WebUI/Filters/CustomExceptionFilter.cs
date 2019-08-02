using System;
using System.Net;
using Core3.Application.Exceptions;
using Core3.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Core3.WebUI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            Guard.ArgumentNotNull(logger, nameof(logger));

            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    ((ValidationException)context.Exception).Failures);
            }
            else
            {
                HttpStatusCode code = HttpStatusCode.InternalServerError;

                if (context.Exception is NotFoundException)
                {
                    code = HttpStatusCode.NotFound;
                }
                else if (context.Exception is BadRequestException)
                {
                    code = HttpStatusCode.BadRequest;
                }

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)code;
                context.Result = new JsonResult(new
                {
                    error = new[] { context.Exception.Message},
                    stackTrace = context.Exception.StackTrace
                });

                _logger.LogError($"{context.Exception.Message}|{context.Exception.StackTrace}");
            }
        }
    }
}
