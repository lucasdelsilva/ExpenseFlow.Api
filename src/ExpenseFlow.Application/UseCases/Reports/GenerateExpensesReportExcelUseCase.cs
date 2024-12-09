using ClosedXML.Excel;
using ExpenseFlow.Application.UseCases.Reports.Interfaces;
using ExpenseFlow.Communication.Enums.Expenses;
using ExpenseFlow.Domain.Reports;
using ExpenseFlow.Domain.Repositories.Expenses;


namespace ExpenseFlow.Application.UseCases.Reports;
public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    //private const string CURRENCY_SYMBOL = ResourceReportMessages.CURRENCY_SYMBOL;
    private readonly IExpensesReadOnlyRepository _repository;
    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository) => _repository = repository;

    public async Task<byte[]> GenerateExcelFile(DateOnly date)
    {
        var expenses = await _repository.FilterByMonth(date);
        if (expenses.Count.Equals(0))
            return [];

        using var workbook = new XLWorkbook { Author = "Lucas", };
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Calibri";

        var workSheet = workbook.Worksheets.Add(date.ToString("Y"));
        InsertHeader(workSheet);

        var raw = 2;
        foreach (var item in expenses)
        {
            workSheet.Cell($"A{raw}").Value = item.Title;
            workSheet.Cell($"B{raw}").Value = item.Date;
            workSheet.Cell($"C{raw}").Value = ConvertPaymentType(item.PaymentType);

            workSheet.Cell($"D{raw}").Value = item.Amount;
            workSheet.Cell($"D{raw}").Style.NumberFormat.Format = $"- #,##0.00";

            workSheet.Cell($"E{raw}").Value = item.Description;

            raw++;
        }

        workSheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private string ConvertPaymentType(PaymentType payment)
    {
        return payment switch
        {
            PaymentType.Cash => ResourceReportMessages.PAYMENT_TYPE_CASH,
            PaymentType.CreditCard => ResourceReportMessages.PAYMENT_TYPE_CREDITCARD,
            PaymentType.DebitCard => ResourceReportMessages.PAYMENT_TYPE_DEBITCARD,
            PaymentType.EletronicTranfers => ResourceReportMessages.PAYMENT_TYPE_ELETRONIC_TRANFERS,
            PaymentType.Other => ResourceReportMessages.PAYMENT_TYPE_OTHERS,
            _ => string.Empty
        };
    }

    private void InsertHeader(IXLWorksheet workSheet)
    {
        workSheet.Cells("A1:E1").Style.Font.Bold = true;
        workSheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#f7dc6f");

        workSheet.Cells("A1:C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        workSheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        workSheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        workSheet.Cell("A1").Value = ResourceReportMessages.TITLE;
        workSheet.Cell("B1").Value = ResourceReportMessages.DATE;
        workSheet.Cell("C1").Value = ResourceReportMessages.PAYMENT_TYPE;
        workSheet.Cell("D1").Value = ResourceReportMessages.AMOUNT;
        workSheet.Cell("E1").Value = ResourceReportMessages.DESCRIPTION;
    }
}