using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Repositories.User;
using ExpenseFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Infrastructure.Repositories;
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email.Equals(email.ToLower()));
    }

    public async Task<User> GetById(long id)
    {
        return await _dbContext.Users.FirstAsync(u => u.Id.Equals(id));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.ToLower().Equals(email.ToLower()));
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }
}