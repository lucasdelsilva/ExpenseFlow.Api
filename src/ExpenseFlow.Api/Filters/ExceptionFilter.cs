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
        var exceptionExpense = (ExpenseFlowException)context.Exception;
        var errorResponse = new ResponseErrorJson(exceptionExpense!.GetErros());

        context.HttpContext.Response.StatusCode = exceptionExpense.StatusCode;
        context.Result = new ObjectResult(errorResponse);
    }

    private static void ThrowUnknownError(ExceptionContext context)
    {
        var message = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(message);
    }
}