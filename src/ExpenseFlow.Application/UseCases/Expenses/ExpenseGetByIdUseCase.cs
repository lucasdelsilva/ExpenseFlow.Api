using AutoMapper;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Domain.Services.LoggedUser;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseGetByIdUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper, ILoggedUser loggedUser) : IExpenseGetByIdUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesRepository = expensesRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseExpenseJson> GetById(long id)
    {
        var loggedUser = await _loggedUser.Get();
        var expense = await _expensesRepository.GetById(loggedUser, id);
        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return _mapper.Map<ResponseExpenseJson>(expense);
    }
}