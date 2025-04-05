using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTests.Repositories;
public class ExpensesReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesReadOnlyRepository> _repository;
    public ExpensesReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesReadOnlyRepository>();
    }

    public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
    {
        _repository.Setup(rep => rep.GetAll(user)).ReturnsAsync(expenses);
        return this;
    }

    public ExpensesReadOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null)
            _repository.Setup(rep => rep.GetById(user, expense.Id)).ReturnsAsync(expense);

        return this;
    }

    public IExpensesReadOnlyRepository Build() => _repository.Object;
}