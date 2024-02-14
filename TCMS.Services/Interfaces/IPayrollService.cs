using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IPayrollService
{
    Task<IEnumerable<Payroll>> GetAllPayrollsAsync();
    Task<Payroll> GetPayrollByIdAsync(int id);
    Task<Payroll> CreatePayrollAsync(Payroll payroll);
    Task<bool> UpdatePayrollAsync(Payroll payroll);
    Task<bool> DeletePayrollAsync(int id);
    Task<bool> CalculateGrossPayAsync(int id);
    Task<bool> CalculateNetPayAsync(int id);
    Task<bool> CalculateDeductionsAsync(int id);
    Task<IEnumerable<Payroll>> GeneratePayrollReportAsync (DateTime startDate, DateTime endDate);
}