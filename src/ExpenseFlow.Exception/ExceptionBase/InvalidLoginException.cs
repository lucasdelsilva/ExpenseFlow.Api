
using System.Net;

namespace ExpenseFlow.Exception.ExceptionBase;
public class InvalidLoginException : ExpenseFlowException
{
    public InvalidLoginException(string message) : base(message) { }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<object> GetErros()
    {
        return [Message];
    }
}
