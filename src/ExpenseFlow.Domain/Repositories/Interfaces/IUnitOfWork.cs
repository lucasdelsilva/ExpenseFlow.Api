namespace ExpenseFlow.Domain.Repositories.Interfaces;
public interface IUnitOfWork
{
    Task Commit();
}