using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Application.UseCases.Reports.Interfaces;
public interface IGenerateExpensesReportExcelUseCase
{
    Task<byte[]> GenerateExcelFile(DateOnly date);
}
