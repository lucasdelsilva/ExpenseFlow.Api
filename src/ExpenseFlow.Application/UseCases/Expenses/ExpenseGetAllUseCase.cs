using AutoMapper;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Repositories.Expenses;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseGetAllUseCase : IExpenseGetAllUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesRepository;
    private readonly IMapper _mapper;
    public ExpenseGetAllUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper)
    {
        _expensesRepository = expensesRepository;
        _mapper = mapper;
    }

    public async Task<ResponseExpensesJson> GetAll()
    {
        var expenses = await _expensesRepository.GetAll();

        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(expenses)
        };
    }
}