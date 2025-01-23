using ManagementSystem.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ManagementSystem.Web.Filter;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException (ExceptionContext context)
    {
        var statusCode = context.Exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            Ardalis.GuardClauses.NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var response = new
        {
            StatusCode = statusCode,
            Message = context.Exception.Message,
            Errors = context.Exception is ValidationException validationException
            ? validationException.Errors
            : null
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}
