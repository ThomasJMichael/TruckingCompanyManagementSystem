using Microsoft.AspNetCore.Mvc;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.Operations;
using TCMS.Services.Interfaces;

namespace TCMS.API.Controllers
{
    [Route("api/shipment")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        // Create incoming
        [HttpPost("create-incoming")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CreateIncomingShipment([FromBody] IncomingShipmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shipmentService.CreateIncomingShipment(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Create outgoing
        [HttpPost("create-outgoing")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> CreateOutgoingShipment([FromBody] OutgoingShipmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shipmentService.CreateOutgoingShipment(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all shipments
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<ShipmentDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<ShipmentDetailDto>>>> GetAllShipments()
        {
            var result = await _shipmentService.GetAllShipmentsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all incoming shipments
        [HttpGet("all-incoming")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<ShipmentDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<ShipmentDetailDto>>>> GetAllIncomingShipments()
        {
            var result = await _shipmentService.GetAllIncomingShipmentsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get all outgoing shipments
        [HttpGet("all-outgoing")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<IEnumerable<ShipmentDetailDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<IEnumerable<ShipmentDetailDto>>>> GetAllOutgoingShipments()
        {
            var result = await _shipmentService.GetAllOutgoingShipmentsAsync();
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Get shipment by id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult<ShipmentDetailDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult<ShipmentDetailDto>>> GetShipmentById(int id)
        {
            var result = await _shipmentService.GetShipmentByIdAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Update shipment
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> UpdateShipment([FromBody] ShipmentUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _shipmentService.UpdateShipmentAsync(dto);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }

        // Delete shipment
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OperationResult>> DeleteShipment(int id)
        {
            var result = await _shipmentService.DeleteShipmentAsync(id);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
    }
}
