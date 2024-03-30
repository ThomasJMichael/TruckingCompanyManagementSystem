using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<VehicleDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<VehicleDto>>>> GetAllVehicles()
        {
            var result = await _vehicleService.GetAllAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<VehicleDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<VehicleDto>>> GetVehicleById(string id)
        {
            var result = await _vehicleService.GetVehicleByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CreateVehicle([FromBody] VehicleCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.CreateVehicleAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateVehicle([FromBody] VehicleUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.UpdateVehicleAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteVehicle(string id)
        {
            var result = await _vehicleService.DeleteVehicleAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get maintenance history of a vehicle
        [HttpGet("maintenance-history/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<MaintenanceRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<MaintenanceRecordDto>>>> GetMaintenanceHistory(string id)
        {
            var result = await _vehicleService.GetMaintenanceRecordsByVehicleIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get Inpection history of a vehicle
        [HttpGet("inspection-history/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<MaintenanceRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<MaintenanceRecordDto>>>> GetInspectionHistory(string id)
        {
            var result = await _vehicleService.GetInspectionRecordsByVehicleIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add maintenance record to a vehicle
        [HttpPost("add-maintenance-record")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddMaintenanceRecord([FromBody] MaintenanceRecordCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.AddMaintenanceRecordToVehicleAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add inspection record to a vehicle
        [HttpPost("add-inspection-record")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddInspectionRecord([FromBody] InspectionRecordCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.AddInspectionRecordToVehicleAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get Repair Records of a vehicle
        [HttpGet("repair-records/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<RepairRecordDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<RepairRecordDto>>>> GetRepairRecords(string id)
        {
            var result = await _vehicleService.GetRepairRecordsByVehicleIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add repair record to a vehicle
        [HttpPost("add-repair-record")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddRepairRecord([FromBody] RepairRecordCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.AddRepairRecordToVehicleAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update repair record of a vehicle
        [HttpPut("update-repair-record")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateRepairRecord([FromBody] RepairRecordUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.UpdateRepairRecordAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete repair record of a vehicle
        [HttpDelete("delete-repair-record/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteRepairRecord(int id)
        {
            var result = await _vehicleService.DeleteRepairRecordAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all parts by vehicle id
        [HttpGet("parts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<PartDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<PartDetailDto>>>> GetParts(string id)
        {
            var result = await _vehicleService.GetPartsByVehicleIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Add part to a vehicle
        [HttpPost("add-part")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> AddPart([FromBody] PartDetailCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.AddPartToVehicleAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Remove part from a vehicle
        [HttpDelete("remove-part/{vehicleId}/{partId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> RemovePart(string vehicleId, int partId)
        {
            var result = await _vehicleService.RemovePartFromVehicleAsync(vehicleId, partId);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all special ordered parts in general
        [HttpGet("special-ordered-parts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<PartDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<PartDetailDto>>>> GetSpecialOrderedParts()
        {
            var result = await _vehicleService.GetAllSpecialOrderedPartsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get special ordered parts by vehicle
        [HttpGet("special-ordered-parts/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<PartDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<PartDetailDto>>>> GetSpecialOrderedPartsByVehicle(string id)
        {
            var result = await _vehicleService.GetSpecialOrderedPartsByVehicleIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Special ordered parts update
        [HttpPut("special-ordered-parts/update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateSpecialOrderedPart([FromBody] SpecialOrderPartUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _vehicleService.SpecialOrderedPartUpdate(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}
