using ExpenseFlow.Application.UseCases.Reports.Interfaces;
using ExpenseFlow.Domain.Reports;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ExpenseFlow.Api.Controllers;
[Route("api/report")]
[ApiController]
public class ReportController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromServices] IGenerateExpensesReportExcelUseCase useCase, [FromHeader] DateOnly date)
    {
        byte[] file = await useCase.GenerateExcelFile(date);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }

    [HttpGet("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf([FromServices] IGenerateExpensesReportPdfUseCase useCase, [FromHeader] DateOnly date)
    {
        byte[] file = await useCase.GeneratePdfFile(date);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, $"{ResourceReportMessages.EXPENSES} - {date:MMMMyyyy}.pdf");

        return NoContent();
    }
}