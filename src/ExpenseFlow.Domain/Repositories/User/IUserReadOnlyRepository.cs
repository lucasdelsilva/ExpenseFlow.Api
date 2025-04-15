
namespace ExpenseFlow.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<Entities.User> GetById(long id);
    Task<Entities.User?> GetUserByEmail(string email);
    void Update(Entities.User user);
}