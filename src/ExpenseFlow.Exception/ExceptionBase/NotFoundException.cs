using System.Net;

namespace ExpenseFlow.Exception.ExceptionBase;
public class NotFoundException : ExpenseFlowException
{
    public NotFoundException(string message) : base(message) { }
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<object> GetErros()
    {
        return [Message];
    }
}