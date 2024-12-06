using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseDeleteUseCase : IExpenseDeleteUseCase
{
    private readonly IExpensesWriteOnlyRepository _expensesRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ExpenseDeleteUseCase(IExpensesWriteOnlyRepository expensesRepository, IUnitOfWork unitOfWork)
    {
        _expensesRepository = expensesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Delete(long id)
    {
        var result = await _expensesRepository.Delete(id);

        if (!result)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _unitOfWork.Commit();
    }
}