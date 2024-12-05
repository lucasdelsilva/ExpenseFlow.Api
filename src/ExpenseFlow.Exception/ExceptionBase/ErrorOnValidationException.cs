using System.Net;

namespace ExpenseFlow.Exception.ExceptionBase;
public class ErrorOnValidationException : ExpenseFlowException
{
    private List<object> Erros { get; set; }
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorOnValidationException(List<object> errorMessages) : base(string.Empty) => Erros = errorMessages;

    public override List<object> GetErros()
    {
        return Erros;
    }
}