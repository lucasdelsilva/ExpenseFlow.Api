using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseCreatedUseCase : IExpenseCreatedUserCase
{
    private readonly IExpensesRepository _expensesRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ExpenseCreatedUseCase(IExpensesRepository expensesRepository, IUnitOfWork unitOfWork)
    {
        _expensesRepository = expensesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseExpensesCreatedModel> Execute(RequestExpensesCreatedModel request)
    {
        ValidatorModel(request);

        var expense = new Expense
        {
            Amount = request.Amount,
            Date = DateTime.UtcNow.AddDays(-1),
            Description = request.Description,
            PaymentType = request.PaymentType,
            Title = request.Title
        };

        _expensesRepository.Create(expense);
        _unitOfWork.Commit();

        return await Task.FromResult(new ResponseExpensesCreatedModel());
    }

    private void ValidatorModel(RequestExpensesCreatedModel request)
    {
        var validator = new ExpenseCreatedValidator();

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
