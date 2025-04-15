using ExpenseFlow.Application.UseCases.User.Interface;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Communication.Response.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseFlow.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase, [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Register(request);
        return Created(string.Empty, response);
    }

    [HttpGet("profile")]
    [Authorize]
    [ProducesResponseType(typeof(ResponseProfileUserJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile([FromServices] IGetProfileUserUseCase useCase)
    {
        var response = await useCase.GetProfile();
        return Ok(response);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> UpdateProfile([FromServices] IUpdateProfileUserUseCase useCase, RequestUpdateProfileUserJson request)
    {
        await useCase.UpdateProfile(request);
        return NoContent();
    }
}