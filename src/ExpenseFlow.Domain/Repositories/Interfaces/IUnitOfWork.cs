namespace ExpenseFlow.Domain.Repositories.Interfaces;
public interface IUnitOfWork
{
    void Commit();
}