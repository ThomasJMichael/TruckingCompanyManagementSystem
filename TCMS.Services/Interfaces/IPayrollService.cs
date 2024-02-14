using TCMS.Common.DTOs.Financial;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IPayrollDtoService
{
    Task<IEnumerable<PayrollDto>> GetAllPayrollsAsync();
    Task<PayrollDto> GetPayrollByIdAsync(int id);
    Task<PayrollDto> CreatePayrollAsync(PayrollDto payroll);
    Task<bool> UpdatePayrollDtoAsync(PayrollDto payroll);
    Task<bool> DeletePayrollDtoAsync(int id);
    Task<bool> CalculateGrossPayAsync(int id);
    Task<bool> CalculateNetPayAsync(int id);
    Task<bool> CalculateDeductionsAsync(int id);
    Task<IEnumerable<PayrollDto>> GeneratePayrollReportAsync (DateTime startDate, DateTime endDate);
}