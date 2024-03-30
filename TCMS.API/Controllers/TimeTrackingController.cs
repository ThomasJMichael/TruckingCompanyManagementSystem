using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/time-tracking")]
    [ApiController]
    public class TimeTrackingController : ControllerBase
    {
        private readonly ITimeTrackingService _timeTrackingService;

        public TimeTrackingController(ITimeTrackingService timeTrackingService)
        {
            _timeTrackingService = timeTrackingService;
        }

        // Clock in
        [HttpPost("clock-in")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> ClockIn()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _timeTrackingService.ClockInAsync(userId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Clock out
        [HttpPost("clock-out")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> ClockOut()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _timeTrackingService.ClockOutAsync(userId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all time sheets
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<TimesheetDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<TimesheetDto>>>> GetAllTimeSheets(GetTimeSheetsDto requestDto)
        {
            var result = await _timeTrackingService.GetTimesheetsForPeriodAsync(requestDto.EmployeeId, requestDto.StartDate, requestDto.EndDate);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}
