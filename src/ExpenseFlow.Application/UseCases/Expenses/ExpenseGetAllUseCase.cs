using AutoMapper;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Domain.Services.LoggedUser;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseGetAllUseCase : IExpenseGetAllUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesRepository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public ExpenseGetAllUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper, ILoggedUser loggedUser)
    {
        _expensesRepository = expensesRepository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseExpensesJson> GetAll()
    {
        var loggedUser = await _loggedUser.Get(); ;
        var expenses = await _expensesRepository.GetAll(loggedUser);

        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(expenses)
        };
    }
}