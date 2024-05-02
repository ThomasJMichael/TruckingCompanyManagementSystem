using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/maintenance")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenanceController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        // Get all maintenance records
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<MaintenanceRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<MaintenanceRecordDto>>>> GetAllMaintenance()
        {
            var result = await _maintenanceService.GetAllMaintenanceRecordsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get maintenance record by id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<MaintenanceRecordDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<MaintenanceRecordDto>>> GetMaintenanceById(int id)
        {
            var result = await _maintenanceService.GetMaintenanceRecordByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Create a new maintenance record
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<MaintenanceRecordDto>>> CreateMaintenance([FromBody] MaintenanceRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _maintenanceService.CreateMaintenanceRecordAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update a maintenance record
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateMaintenance([FromBody] MaintenanceRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _maintenanceService.UpdateMaintenanceRecordAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete a maintenance record
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteMaintenance(int id)
        {
            var result = await _maintenanceService.DeleteMaintenanceRecordAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all parts for a given maintenance record
        [HttpGet("all-parts/{maintenanceRecordId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<PartDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<PartDetailDto>>>> GetPartsByMaintenanceRecord(int maintenanceRecordId)
        {
            var result = await _maintenanceService.GetPartsByMaintenanceRecordIdAsync(maintenanceRecordId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPost("update-parts/{maintenanceRecordId}")]
        public async Task<ActionResult<OperationResult>> UpdateParts(int maintenanceRecordId, [FromBody] PartsDto partsDto)
        {
            var result = await _maintenanceService.UpdateParts(maintenanceRecordId, partsDto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }


        // Get all maintenance records for a given period
        [HttpGet("period")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<MaintenanceRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<MaintenanceRecordDto>>>> GetMaintenanceForPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _maintenanceService.GetMaintenanceRecordsForPeriod(startDate, endDate);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all repair records
        [HttpGet("repairs/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<RepairRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<RepairRecordDto>>>> GetAllRepairs()
        {
            var result = await _maintenanceService.GetAllRepairRecordsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get repair record by id
        [HttpGet("repairs/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<RepairRecordDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<RepairRecordDto>>> GetRepairById(int id)
        {
            var result = await _maintenanceService.GetRepairRecordByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Create a new repair record
        [HttpPost("repairs/create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CreateRepair([FromBody] RepairRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _maintenanceService.CreateRepairRecordAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update a repair record
        [HttpPut("repairs/update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateRepair([FromBody] RepairRecordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _maintenanceService.UpdateRepairRecordAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete a repair record
        [HttpDelete("repairs/delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteRepair(int id)
        {
            var result = await _maintenanceService.DeleteRepairRecordAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all repair records for a given vehicle
        [HttpGet("repairs/vehicle")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<RepairRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<RepairRecordDto>>>> GetRepairsByVehicle([FromQuery] int vehicleId)
        {
            var result = await _maintenanceService.GetRepairRecordsByVehicleIdAsync(vehicleId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all parts
        [HttpGet("parts/all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<PartDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<PartDetailDto>>>> GetAllParts()
        {
            var result = await _maintenanceService.GetAllPartsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get part by id
        [HttpGet("parts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<PartDetailDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<PartDetailDto>>> GetPartById(int id)
        {
            var result = await _maintenanceService.GetPartByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add a new part
        [HttpPost("parts/add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddPart([FromBody] PartDetailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _maintenanceService.AddPartAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update a part
        [HttpPut("parts/update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdatePart([FromBody] PartDetailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _maintenanceService.UpdatePartAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete a part
        [HttpDelete("parts/delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeletePart(int id)
        {
            var result = await _maintenanceService.DeletePartAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Assign a part to a maintenance record
        [HttpPost("parts/assign")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AssignPartToMaintenance([FromQuery] int maintenanceRecordId,
            [FromQuery] int partId)
        {
            var result = await _maintenanceService.AssignPartToMaintenanceRecordAsync(maintenanceRecordId, partId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add a special ordered part
        [HttpPost("parts/special-order")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddSpecialOrderedPart([FromBody] PartDetailDto dto,
            [FromQuery] string orderSource, [FromQuery] decimal cost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _maintenanceService.AddSpecialOrderedPartAsync(dto, orderSource, cost);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}
