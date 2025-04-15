using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommonTests.Entities
{
    public class LoggedUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            var mock = new Mock<ILoggedUser>();

            mock.Setup(m => m.Get()).ReturnsAsync(user);
            return mock.Object;
        }
    }
}