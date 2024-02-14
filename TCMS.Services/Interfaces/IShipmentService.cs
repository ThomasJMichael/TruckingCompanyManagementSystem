using TCMS.Common.DTOs.Shipment;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IShipmentService
{
    Task<Shipment> CreateIncomingShipment(IncomingShipmentDto incomingShipmentDto);
    Task<Shipment> CreateOutgoingShipment(OutgoingShipmentDto outgoingShipmentDto);
    Task<IEnumerable<Shipment>> GetAllShipmentsAsync();
    Task<IEnumerable<Shipment>> GetAllIncomingShipmentsAsync();
    Task<IEnumerable<Shipment>> GetAllOutgoingShipmentsAsync();
    Task<Shipment> GetShipmentByIdAsync(int id);
    Task<bool> UpdateShipmentAsync(Shipment shipment);
    Task<bool> DeleteShipmentAsync(int id);



}