using TCMS.Common.DTOs.Financial;
using TCMS.Common.Operations;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IPayrollService
{
    // Retrieves all payroll records
    Task<OperationResult<IEnumerable<PayrollDto>>> GetAllPayrollsAsync();

    // Retrieves a specific payroll record by ID
    Task<OperationResult<PayrollDto>> GetPayrollByIdAsync(int id);

    // Generates payroll for all employees based on timesheets within a given date range
    Task<OperationResult<IEnumerable<PayrollDto>>> GeneratePayrollForPeriodAsync(DateTime payPeriodStart, DateTime payPeriodEnd);

    // Updates an existing payroll record (optional, depends on business logic)
    Task<OperationResult> UpdatePayrollAsync(PayrollDto payroll);

    // Deletes a specific payroll record by ID (optional, depends on business logic)
    Task<OperationResult> DeletePayrollAsync(int id);

    // Additional helper methods to calculate parts of the payroll
    // Task<OperationResult<decimal>> CalculateGrossPayAsync(int employeeId, DateTime startDate, DateTime endDate);
    // Task<OperationResult<decimal>> CalculateNetPayAsync(int payrollId);
    // Task<OperationResult<decimal>> CalculateDeductionsAsync(int payrollId);

    // Generates a report for payroll within a specific period
    Task<OperationResult<IEnumerable<PayrollDto>>> GeneratePayrollReportAsync(DateTime startDate, DateTime endDate);
}

