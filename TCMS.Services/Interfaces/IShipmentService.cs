using TCMS.Common.DTOs.Shipment;
using TCMS.Common.Operations;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IShipmentService
{
    Task<OperationResult<ShipmentCreateDto>> CreateIncomingShipment(IncomingShipmentDto incomingShipmentDto);
    Task<OperationResult<ShipmentCreateDto>> CreateOutgoingShipment(OutgoingShipmentDto outgoingShipmentDto);
    Task<OperationResult<IEnumerable<ShipmentDetailDto>>> GetAllShipmentsAsync();
    Task<OperationResult<IEnumerable<ShipmentDetailDto>>> GetAllIncomingShipmentsAsync();
    Task<OperationResult<IEnumerable<ShipmentDetailDto>>> GetAllOutgoingShipmentsAsync();
    Task<OperationResult<ShipmentDetailDto>> GetShipmentByIdAsync(int id);
    Task<OperationResult> UpdateShipmentAsync(ShipmentUpdateDto shipmentUpdateDto);
    Task<OperationResult> DeleteShipmentAsync(int id);
}
