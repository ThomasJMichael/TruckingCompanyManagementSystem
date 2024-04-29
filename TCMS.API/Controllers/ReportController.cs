using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Report;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // Payroll
        [HttpPost("payroll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<PayrollReportDto>>>> GeneratePayrollReport([FromBody] ReportRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reportService.GeneratePayrollReport(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Maintenance
        [HttpPost("maintenance")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<MaintenanceReportDto>>>> GenerateMaintenanceReport([FromBody] ReportRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reportService.GenerateMaintenanceReport(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Vehicle maintenance
        [HttpGet("vehicle-maintenance")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> GenerateVehicleMaintenanceReport(
            [FromBody] ReportRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reportService.GenerateVehicleMaintenanceReport(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Incoming shipments
        [HttpGet("incoming-shipments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> GenerateIncomingShipmentsReport()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reportService.GenerateIncomingShipmentsReport();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Outgoing shipments
        [HttpGet("outgoing-shipments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> GenerateOutgoingShipmentsReport()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _reportService.GenerateOutgoingShipmentsReport();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

    }
}
