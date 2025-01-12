using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Domain.Services.LoggedUser;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseDeleteUseCase : IExpenseDeleteUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
    private readonly IExpensesWriteOnlyRepository _expensesWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    public ExpenseDeleteUseCase(IExpensesWriteOnlyRepository expensesRepository, IExpensesReadOnlyRepository expensesReadOnlyRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _expensesWriteOnlyRepository = expensesRepository;
        _expensesReadOnlyRepository = expensesReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Delete(long id)
    {
        var loggedUser = await _loggedUser.Get();
        var expense = await _expensesReadOnlyRepository.UpdateOrRemoveGetById(loggedUser, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _expensesWriteOnlyRepository.Delete(id);

        await _unitOfWork.Commit();
    }
}