namespace ExpenseFlow.Communication.Response.Errors;
public class ResponseErrors
{
    public List<Error> Errors { get; set; } = [];
}

public class Error
{
    public string ErrorMessage { get; set; } = string.Empty;
    public string NameProperty { get; set; } = string.Empty;
}