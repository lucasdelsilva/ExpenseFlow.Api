using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{

    [HttpPost]
    public IActionResult Post([FromBody] RequestExpensesCreatedModel request)
    {
        try
        {
            var useCase = new ExpenseCreatedUseCase();
            var response = useCase.Execute(request);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            
            return BadRequest(ex);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
        }
    }
}