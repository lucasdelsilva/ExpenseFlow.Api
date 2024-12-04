using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Communication.Response.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.Api.Controllers;

[Route("api/expenses")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseExpensesCreatedModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Created([FromServices] IExpenseCreatedUserCase useCase, [FromBody] RequestExpensesCreatedModel request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}