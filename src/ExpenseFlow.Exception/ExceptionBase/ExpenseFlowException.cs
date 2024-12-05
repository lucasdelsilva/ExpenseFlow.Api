namespace ExpenseFlow.Exception.ExceptionBase;
public abstract class ExpenseFlowException : SystemException
{
    protected ExpenseFlowException(string message) : base(message) { }
}