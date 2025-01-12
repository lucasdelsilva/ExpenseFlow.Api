using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    Task<User> Get();
}
