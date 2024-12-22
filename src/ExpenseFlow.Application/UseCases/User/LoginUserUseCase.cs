using ExpenseFlow.Application.UseCases.User.Interface;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Repositories.User;
using ExpenseFlow.Domain.Security.Cryptography;
using ExpenseFlow.Domain.Security.Tokens;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.User;
public class LoginUserUseCase : ILoginUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginUserUseCase(IUserReadOnlyRepository userReadOnlyRepository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseRegisterUserJson> Login(RequestLoginUserJson request)
    {
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);
        if (user is null)
            throw new InvalidLoginException(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);

        var passwordMatch = _passwordEncripter.VerificationPassword(request.Password, user.Password);
        if (!passwordMatch)
            throw new InvalidLoginException(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.GeneratorToken(user)
        };
    }
}