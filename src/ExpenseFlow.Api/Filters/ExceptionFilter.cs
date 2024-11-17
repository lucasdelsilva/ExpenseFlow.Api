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

    private void HandleProjectExeption(ExceptionContext context)
    {
        if (context.Exception is ErrorOnValidationException)
        {
            var ex = (ErrorOnValidationException)context.Exception;
            var errorMessage = new ResponseErrorModel(ex.Erros);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorMessage);
        }
        else
        {
            var errorMessage = new ResponseErrorModel(context.Exception.Message);

            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorMessage);
        }
    }

    private void ThrowUnknownError(ExceptionContext context)
    {
        var message = new ResponseErrorModel(ResourceErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(message);
    }
}
