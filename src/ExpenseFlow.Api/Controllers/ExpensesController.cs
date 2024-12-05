using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Communication.Response.Errors;
using ExpenseFlow.Exception;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.Api.Controllers;

[Route("api/expenses")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseExpensesCreatedJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Created([FromServices] IExpenseCreateUseCase useCase, [FromBody] RequestExpensesCreatedModel request)
    {
        var response = await useCase.Create(request);
        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseExpensesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorModel), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IExpenseGetAllUseCase useCase)
    {
        var response = await useCase.GetAll();
        if (response.Expenses.Count > 0)
            return Ok(response);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseExpenseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IExpenseGetByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.GetById(id);
        if (response is not null)
            return Ok(response);

        return NotFound(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }
}