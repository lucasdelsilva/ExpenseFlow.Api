using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Communication.Request;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    public IActionResult Created([FromBody] RequestExpensesCreatedModel request)
    {
        var useCase = new ExpenseCreatedUseCase();
        var response = useCase.Execute(request);

        return Created(string.Empty, response);
    }
}