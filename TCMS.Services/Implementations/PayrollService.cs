using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class PayrollService : IPayrollService
    {
        private readonly TcmsContext _context;
        private readonly IMapper _mapper;

        public PayrollService(TcmsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<OperationResult<IEnumerable<PayrollDto>>> GetAllPayrollsAsync()
        {
            try
            {
                var payrolls = await _context.Payrolls
                    .Include(p => p.Employee)
                    .ToListAsync();

                var payrollDtos = _mapper.Map<IEnumerable<PayrollDto>>(payrolls);

                return OperationResult<IEnumerable<PayrollDto>>.Success(payrollDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<PayrollDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<PayrollDto>> GetPayrollByIdAsync(int id)
        {
            try
            {
                var payroll = await _context.Payrolls
                    .Include(p => p.Employee)
                    .FirstOrDefaultAsync(p => p.PayrollId == id);

                if (payroll == null)
                    return OperationResult<PayrollDto>.Failure(new List<string> { "Payroll not found" });

                var payrollDto = _mapper.Map<PayrollDto>(payroll);
                return OperationResult<PayrollDto>.Success(payrollDto);
            }
            catch (Exception e)
            {
                return OperationResult<PayrollDto>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<PayrollDto>>> GeneratePayrollForPeriodAsync(
            DateTime payPeriodStart, DateTime payPeriodEnd)
        {
            try
            {
                var timesheets = await _context.TimeSheets
                    .Include(t => t.Employee)
                    .Where(t => t.Date >= payPeriodStart && t.Date <= payPeriodEnd)
                    .ToListAsync();

                var groupedTimeSheets = timesheets.GroupBy(t => t.EmployeeId);

                var payrollList = new List<PayrollDto>();

                foreach (var group in groupedTimeSheets)
                {
                    var employee = await _context.Employees.FindAsync(group.Key);
                    if (employee == null)
                        return OperationResult<IEnumerable<PayrollDto>>.Failure(new List<string>
                            { "Employee not found" });

                    var payroll = new PayrollDto
                    {
                        EmployeeId = employee.EmployeeId,
                        FirstName = employee.FirstName,
                        MiddleName = employee.MiddleName ?? "",
                        LastName = employee.LastName,
                        HoursWorked = group.Sum(ts => ts.HoursWorked),
                        PayRate = employee.PayRate,
                        PayPeriodStart = payPeriodStart,
                        PayPeriodEnd = payPeriodEnd,
                        GrossPay = CalculateGrossPay(employee, group),
                        TaxDeductions = CalculateTaxDeductions(employee, group),
                        OtherDeductions = CalculateOtherDeductions(employee),
                        NetPay = CalculateNetPay(CalculateGrossPay(employee, group), CalculateTaxDeductions(employee, group),
                            CalculateOtherDeductions(employee))
                    };

                    payrollList.Add(payroll);

                }
                return OperationResult<IEnumerable<PayrollDto>>.Success(payrollList);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<PayrollDto>>.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> UpdatePayrollAsync(PayrollDto payroll)
        {
            try
            {
                var payrollToUpdate = await _context.Payrolls.FindAsync(payroll.PayrollId);
                if (payrollToUpdate == null)
                    return OperationResult.Failure(new List<string> { "Payroll not found" });

                _mapper.Map(payroll, payrollToUpdate);
                await _context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult> DeletePayrollAsync(int id)
        {
            try
            {
                var payroll = await _context.Payrolls.FindAsync(id);
                if (payroll == null)
                    return OperationResult.Failure(new List<string> { "Payroll not found" });

                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new List<string> { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<PayrollDto>>> GeneratePayrollReportAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var employees = await _context.Employees.Include(e => e.TimeSheets).ToListAsync();

                var payrollReport = new List<PayrollDto>();

                foreach (var employee in employees)
                {
                    var relevantTimeSheets = employee.TimeSheets
                        .Where(ts => ts.Date >= startDate && ts.Date <= endDate);

                    if (!relevantTimeSheets.Any()) continue; // Skip if no timesheets in period

                    decimal totalHoursWorked = relevantTimeSheets.Sum(ts => ts.HoursWorked);
                    decimal grossPay = CalculateGrossPay(employee, totalHoursWorked);
                    decimal taxDeduction = CalculateTaxDeductions(employee, totalHoursWorked);
                    decimal otherDeductions = CalculateOtherDeductions(employee, totalHoursWorked);
                    decimal netPay = grossPay - taxDeduction - otherDeductions;

                    payrollReport.Add(new PayrollDto
                    {
                        EmployeeId = employee.EmployeeId,
                        FirstName = employee.FirstName,
                        MiddleName = employee.MiddleName ?? "",
                        LastName = employee.LastName,
                        HoursWorked = totalHoursWorked,
                        PayRate = employee.PayRate,
                        PayPeriodStart = startDate,
                        PayPeriodEnd = endDate,
                        GrossPay = grossPay,
                        TaxDeductions = taxDeduction,
                        OtherDeductions = otherDeductions,
                        NetPay = netPay
                    });
                }

                return OperationResult<IEnumerable<PayrollDto>>.Success(payrollReport);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<PayrollDto>>.Failure(new List<string> { e.Message });
            }
        }


        private static decimal CalculateGrossPay(Employee employee, IGrouping<string, TimeSheet> timeSheetsGroup)
        {
            decimal regularPay = CalculateRegularPay(employee, timeSheetsGroup);
            decimal overtimePay = CalculateOvertimePay(employee, timeSheetsGroup);
            return regularPay + overtimePay;
        }

        private decimal CalculateGrossPay(Employee employee, decimal totalHoursWorked)
        {
            decimal regularPay = CalculateRegularPay(employee, totalHoursWorked);
            decimal overtimePay = CalculateOvertimePay(employee, totalHoursWorked);
            return regularPay + overtimePay;
        }

        private static decimal CalculateTaxDeductions(Employee employee, IGrouping<string, TimeSheet> timeSheetsGroup)
        {
            const decimal taxRate = 0.2m;
            decimal grossPay = CalculateGrossPay(employee, timeSheetsGroup);
            return grossPay * taxRate;
        }

        private decimal CalculateTaxDeductions(Employee employee, decimal totalHoursWorked)
        {
            const decimal taxRate = 0.2m;
            decimal grossPay = CalculateGrossPay(employee, totalHoursWorked);
            return grossPay * taxRate;
        }

        private static decimal CalculateOtherDeductions(Employee employee, decimal totalHoursWorked)
        {
            const decimal insuranceDeduction = 100;
            return insuranceDeduction;
        }

        private static decimal CalculateOtherDeductions(Employee employee)
        {
            const decimal insuranceDeduction = 100;
            return insuranceDeduction;
        }

        private static decimal CalculateNetPay(decimal grossPay, decimal taxDeductions, decimal otherDeductions)
        {
            return grossPay - taxDeductions - otherDeductions;
        }

        private static decimal CalculateOvertimePay(Employee employee, IGrouping<string, TimeSheet> timeSheetsGroup)
        { 
            var hourlyRate = employee.PayRate; 
            const decimal overtimeRateMultiplier = 1.5m;
            var overtimeRate = hourlyRate * overtimeRateMultiplier;

            var overtimeHours = timeSheetsGroup.Sum(ts => ts.HoursWorked > 40 ? ts.HoursWorked - 40 : 0);

            return overtimeHours * overtimeRate;
        }

        private decimal CalculateOvertimePay(Employee employee, decimal totalHoursWorked)
        {
            var hourlyRate = employee.PayRate;
            const decimal overtimeRateMultiplier = 1.5m;
            var overtimeRate = hourlyRate * overtimeRateMultiplier;

            var overtimeHours = totalHoursWorked > 40 ? totalHoursWorked - 40 : 0;

            return overtimeHours * overtimeRate;
        }

        private static decimal CalculateRegularPay(Employee employee, IGrouping<string, TimeSheet> timeSheetsGroup)
        {
            var hourlyRate = employee.PayRate;
            var regularHours = timeSheetsGroup.Sum(ts => ts.HoursWorked <= 40 ? ts.HoursWorked : 40);
            return regularHours * hourlyRate;
        }

        private decimal CalculateRegularPay(Employee employee, decimal totalHoursWorked)
        {
            var hourlyRate = employee.PayRate;
            var regularHours = totalHoursWorked <= 40 ? totalHoursWorked : 40;
            return regularHours * hourlyRate;
        }
    }
}
