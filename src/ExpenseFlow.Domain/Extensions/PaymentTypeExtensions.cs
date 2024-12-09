using ExpenseFlow.Domain.Enums;
using ExpenseFlow.Domain.Reports;

namespace ExpenseFlow.Domain.Extensions;
public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportMessages.PAYMENT_TYPE_CASH,
            PaymentType.CreditCard => ResourceReportMessages.PAYMENT_TYPE_CREDITCARD,
            PaymentType.DebitCard => ResourceReportMessages.PAYMENT_TYPE_DEBITCARD,
            PaymentType.EletronicTranfers => ResourceReportMessages.PAYMENT_TYPE_ELETRONIC_TRANFERS,
            PaymentType.Other => ResourceReportMessages.PAYMENT_TYPE_OTHERS,
            _ => string.Empty
        };
    }
}