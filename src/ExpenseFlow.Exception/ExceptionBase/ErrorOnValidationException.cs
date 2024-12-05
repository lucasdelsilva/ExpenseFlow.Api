namespace ExpenseFlow.Exception.ExceptionBase;
public class ErrorOnValidationException : ExpenseFlowException
{
    public List<Object> Erros { get; set; }

    public ErrorOnValidationException(List<Object> errorMessages) : base(string.Empty)
    {
        Erros = errorMessages;
    }
}