using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.DrugTest;
using TCMS.Common.enums;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/drug-test")]
    [ApiController]
    public class DrugTestController : ControllerBase
    {
        private readonly IDrugTestService _drugTestService;

        public DrugTestController(IDrugTestService drugTestService)
        {
            _drugTestService = drugTestService;
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CreateDrugTest([FromBody] DrugTestCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _drugTestService.CreateTestAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<DrugTestDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<DrugTestDto>>>> GetAllDrugTests()
        {
            var result = await _drugTestService.GetAllTestsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<DrugTestDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<DrugTestDto>>> GetDrugTestById(int id)
        {
            var result = await _drugTestService.GetTestByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateDrugTest([FromBody] DrugTestUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _drugTestService.UpdateTestAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteDrugTest(int id)
        {
            var result = await _drugTestService.DeleteTestAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("tests/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<DrugTestDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<DrugTestDto>>>> GetTestsByDriverId(int employeeId)
        {
            var result = await _drugTestService.GetTestsByEmployeeId(employeeId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPost("schedule-followup/{drugTestId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> ScheduleFollowupTest(int drugTestId, [FromBody] DateTime followUpDate)
        {
            var result = await _drugTestService.ScheduleFollowUpTestAsync(drugTestId, followUpDate);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("complete-followup/{drugTestId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CompleteFollowupTest(int drugTestId, [FromBody] TestResult testResult)
        {
            var result = await _drugTestService.CompleteFollowUpTestAsync(drugTestId, testResult);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}

