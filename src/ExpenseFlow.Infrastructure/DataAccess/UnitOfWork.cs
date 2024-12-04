using ExpenseFlow.Domain.Repositories.Interfaces;

namespace ExpenseFlow.Infrastructure.DataAccess;
internal class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}