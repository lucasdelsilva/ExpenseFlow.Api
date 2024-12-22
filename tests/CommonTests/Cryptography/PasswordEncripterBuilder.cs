using ExpenseFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTests.Cryptography;
public class PasswordEncripterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;
    public PasswordEncripterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();
        _mock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("MockPassword1!");
    }

    public PasswordEncripterBuilder Verify(string? password)
    {
        if (!string.IsNullOrWhiteSpace(password))
            _mock.Setup(passwordEncripter => passwordEncripter.VerificationPassword(password, It.IsAny<string>())).Returns(true);


        return this;
    }

    public IPasswordEncripter Build() => _mock.Object;
}