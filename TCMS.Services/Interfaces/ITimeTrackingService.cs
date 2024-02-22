using TCMS.Common.DTOs.Financial;
using TCMS.Common.Operations;

namespace TCMS.Services.Interfaces
{
    public interface ITimeTrackingService
    {
        Task<OperationResult> ClockInAsync(string userId);
        Task<OperationResult> ClockOutAsync(string userId);
        Task<OperationResult<IEnumerable<TimesheetDto>>> GetTimesheetsForPeriodAsync(string employeeId, DateTime startDate,
            DateTime endDate);
    }
}

