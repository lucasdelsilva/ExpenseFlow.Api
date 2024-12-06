using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Domain.Repositories.Expenses;
public interface IExpensesWriteOnlyRepository
{
    Task Create(Expense expense);

    /// <summary>
    /// This fucntion returns TRUE if the deletion was successfull
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Delete(long id);
    void Update(Expense request);
}