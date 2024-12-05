using ExpenseFlow.Communication.Response.Errors;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseFlow.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ExpenseFlowException)
            HandleProjectExeption(context);
        else
            ThrowUnknownError(context);
    }

    private static void HandleProjectExeption(ExceptionContext context)
    {
        if (context.Exception is ErrorOnValidationException erros)
        {
            var errorMessage = new ResponseErrorModel(erros.Erros);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorMessage);
        }
        else if (context.Exception is NotFoundException notFoundException)
        {
            var errorMessage = new ResponseErrorModel(notFoundException.Message);

            context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Result = new BadRequestObjectResult(errorMessage);
        }
        else
        {
            var errorMessage = new ResponseErrorModel(context.Exception.Message);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorMessage);
        }
    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        var message = new ResponseErrorModel(ResourceErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(message);
    }
}
