using AutoMapper;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Application.UseCases.Expenses.Validator;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseUpdateUseCase : IExpenseUpdateUseCase
{
    private readonly IExpensesWriteOnlyRepository _expensesWriteOnlyRepository;
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ExpenseUpdateUseCase(IExpensesWriteOnlyRepository expensesWriteOnlyRepository, IExpensesReadOnlyRepository expensesRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _expensesWriteOnlyRepository = expensesWriteOnlyRepository;
        _expensesReadOnlyRepository = expensesRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Update(long id, RequestExpenseCreateOrUpdateJson request)
    {
        Validate(request);

        var expense = await _expensesReadOnlyRepository.UpdateOrRemoveGetById(id);
        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        _mapper.Map(request, expense);
        _expensesWriteOnlyRepository.Update(expense);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestExpenseCreateOrUpdateJson request)
    {
        var validator = new ExpenseValidator();

        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            var objErros = new List<Object>();
            foreach (var item in result.Errors)
            {
                var objModel = new { item.PropertyName, Message = item.ErrorMessage };
                objErros.Add(objModel);
            }
            throw new ErrorOnValidationException(objErros);
        }
    }
}
