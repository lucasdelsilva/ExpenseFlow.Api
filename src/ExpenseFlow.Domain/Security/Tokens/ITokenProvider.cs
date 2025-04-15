namespace ExpenseFlow.Domain.Security.Tokens;
public interface ITokenProvider
{
    string TokenOnRequest();
}