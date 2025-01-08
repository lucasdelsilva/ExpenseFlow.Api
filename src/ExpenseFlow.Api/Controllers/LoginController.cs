using ExpenseFlow.Application.UseCases.User.Interface;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Communication.Response.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.Api.Controllers;
[Route("api/login")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILoginUserUseCase useCase, [FromBody] RequestLoginUserJson request)
    {
        var response = await useCase.Login(request);
        return Ok(response);
    }
}