using TCMS.Common.DTOs.Financial;
using TCMS.Common.Operations;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IPayrollService
{
    Task<OperationResult<IEnumerable<PayrollDto>>> GetAllPayrollsAsync();
    Task<OperationResult<PayrollDto>> GetPayrollByIdAsync(int id);
    Task<OperationResult<PayrollDto>> CreatePayrollAsync(PayrollDto payroll);
    Task<OperationResult> UpdatePayrollDtoAsync(PayrollDto payroll);
    Task<OperationResult> DeletePayrollDtoAsync(int id);
    Task<OperationResult> CalculateGrossPayAsync(int id);
    Task<OperationResult> CalculateNetPayAsync(int id);
    Task<OperationResult> CalculateDeductionsAsync(int id);
    Task<OperationResult<IEnumerable<PayrollDto>>> GeneratePayrollReportAsync(DateTime startDate, DateTime endDate);
}
