using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseCreatedUseCase
{
    public ResponseExpensesCreatedModel Execute(RequestExpensesCreatedModel request)
    {
        ValidatorModel(request);
        return new ResponseExpensesCreatedModel();
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