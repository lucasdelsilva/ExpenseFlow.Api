using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Communication.Response.Errors;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseCreatedUseCase
{
    public ResponseExpensesCreatedModel Execute(RequestExpensesCreatedModel request)
    {
        var responseModelErrors = new ResponseErrors();

        ValidatorModel(request, responseModelErrors.Errors);
        return new ResponseExpensesCreatedModel();
    }

    private void ValidatorModel(RequestExpensesCreatedModel request, List<Error> errors)
    {
        var validator = new ExpenseCreatedValidator();
        var result = validator.Validate(request);


        foreach (var item in result.Errors)
        {
            var errorList = new Error
            {
                ErrorMessage = item.ErrorMessage,
                NameProperty = item.PropertyName
            };

            errors.Add(errorList);
        }
        newArgumentEx(errors);
    }

    private void newArgumentEx(List<Error> errors)
    {
        throw new NotImplementedException();
    }
}