using ExpenseFlow.Communication.Enums.Expenses;

namespace ExpenseFlow.Communication.Request;

public class RequestExpensesCreatedModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; } = PaymentType.Other;
}