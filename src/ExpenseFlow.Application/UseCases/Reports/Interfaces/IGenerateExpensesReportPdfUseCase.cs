namespace ExpenseFlow.Application.UseCases.Reports.Interfaces;
public interface IGenerateExpensesReportPdfUseCase
{
    Task<byte[]> GeneratePdfFile(DateOnly date);
}