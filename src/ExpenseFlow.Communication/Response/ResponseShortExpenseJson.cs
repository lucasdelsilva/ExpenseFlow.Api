using ExpenseFlow.Communication.Enums;

namespace ExpenseFlow.Communication.Response;
public class ResponseShortExpenseJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public IList<Tag> Tags { get; set; } = [];
}