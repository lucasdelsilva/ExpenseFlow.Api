using ExpenseFlow.Communication.Enums.Expenses;

namespace ExpenseFlow.Communication.Response;
public class ResponseExpenseJson
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
}