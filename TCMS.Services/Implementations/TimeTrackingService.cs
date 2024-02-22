using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.Operations;
using TCMS.Data.Data;
using TCMS.Data.Models;
using TCMS.Services.Interfaces;

namespace TCMS.Services.Implementations
{
    public class TimeTrackingService : ITimeTrackingService
    {
        private readonly TcmsContext _context;
        private readonly IMapper _mapper;
        public async Task<OperationResult> ClockInAsync(string userId)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserAccount.UserName == userId);
                if (employee == null) {
                    return OperationResult.Failure(new[] { "Employee not found." }); }

                var timesheet = new TimeSheet
                {
                    EmployeeId = employee.EmployeeId,
                    Date = DateTime.UtcNow,
                    ClockIn = DateTime.UtcNow,
                };

                _context.TimeSheets.Add(timesheet);
                await _context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }


        public async Task<OperationResult> ClockOutAsync(string userId)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserAccount.UserName == userId);
                if (employee == null)
                {
                    return OperationResult.Failure(new[] { "Employee not found." });
                }

                var timesheet = await _context.TimeSheets
                    .Where(t => t.EmployeeId == employee.EmployeeId && t.Date.Date == DateTime.UtcNow.Date)
                    .OrderByDescending(t => t.ClockIn)
                    .FirstOrDefaultAsync();

                if (timesheet == null)
                {
                    return OperationResult.Failure(new[] { "No timesheet found." });
                }

                timesheet.ClockOut = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return OperationResult.Success();
            }
            catch (Exception e)
            {
                return OperationResult.Failure(new[] { e.Message });
            }
        }

        public async Task<OperationResult<IEnumerable<TimesheetDto>>> GetTimesheetsForPeriodAsync(string employeeId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var timesheets = await _context.TimeSheets
                    .Where(t => t.EmployeeId == employeeId && t.Date.Date >= startDate.Date &&
                                t.Date.Date <= endDate.Date)
                    .ToListAsync();

                var timesheetDtos = _mapper.Map<IEnumerable<TimesheetDto>>(timesheets);
                return OperationResult<IEnumerable<TimesheetDto>>.Success(timesheetDtos);
            }
            catch (Exception e)
            {
                return OperationResult<IEnumerable<TimesheetDto>>.Failure([ e.Message ]);
            }
        }
    }
}
