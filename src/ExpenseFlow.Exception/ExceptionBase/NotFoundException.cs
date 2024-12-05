namespace ExpenseFlow.Exception.ExceptionBase;
public class NotFoundException : ExpenseFlowException
{
    public NotFoundException(string message) : base(message)
    {
    }
}
