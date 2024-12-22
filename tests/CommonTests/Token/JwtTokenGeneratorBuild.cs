using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTests.Token;
public class JwtTokenGeneratorBuild
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(accessTokenGenerator => accessTokenGenerator.GeneratorToken(It.IsAny<User>())).Returns("Token");
        return mock.Object;
    }
}
