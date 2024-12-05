using AutoMapper;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseGetByIdUseCase(IExpensesRepository expensesRepository, IMapper mapper) : IExpenseGetByIdUseCase
{
    private readonly IExpensesRepository _expensesRepository = expensesRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseExpenseJson> GetById(long id)
    {
        var expense = await _expensesRepository.GetById(id);
        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return _mapper.Map<ResponseExpenseJson>(expense);
    }
}