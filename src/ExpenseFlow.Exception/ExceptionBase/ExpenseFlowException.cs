namespace ExpenseFlow.Exception.ExceptionBase;
public abstract class ExpenseFlowException : SystemException
{
    protected ExpenseFlowException(string message) : base(message) { }

    public abstract int StatusCode { get; }
    public abstract List<Object> GetErros();
}