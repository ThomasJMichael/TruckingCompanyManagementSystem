using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/incident")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IIncidentService _incidentService;

        public IncidentController(IIncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CreateIncident([FromBody] IncidentReportDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _incidentService.CreateIncidentReportAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update the incident report
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateIncident([FromBody] IncidentReportDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _incidentService.UpdateIncidentReportAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete the incident report
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteIncident(int id)
        {
            var result = await _incidentService.DeleteIncidentReportAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all incident reports
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<IncidentReportDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<IncidentReportDto>>>> GetAllIncidents()
        {
            var result = await _incidentService.GetAllIncidentReportsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);

        }

        // Get incident report by id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IncidentReportDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IncidentReportDto>>> GetIncidentById(int id)
        {
            var result = await _incidentService.GetIncidentReportByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Evaluate the incident report
        [HttpPut("evaluate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> EvaluateIncident(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _incidentService.EvaluateAndInitiateDrugTestForIncidentAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}
