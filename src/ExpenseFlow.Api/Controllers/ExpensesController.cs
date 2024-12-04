using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Communication.Request;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.Api.Controllers;

[Route("api/expenses")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Created([FromServices] IExpenseCreatedUserCase useCase, [FromBody] RequestExpensesCreatedModel request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}