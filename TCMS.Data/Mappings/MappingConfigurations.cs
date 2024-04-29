using TCMS.Common.DTOs.DrugTest;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.Equipment;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.DTOs.User;
using TCMS.Data.Models;

namespace TCMS.Data.Mappings
{
    public class MappingConfigurations : AutoMapper.Profile
    {
        public MappingConfigurations()
        {
            // DrugTest mappings
            CreateMap<DrugAndAlcoholTest, DrugTestDto>().ReverseMap()
                .ForMember(dest => dest.DrugAndAlcoholTestId, opt => opt.MapFrom(src => src.DrugAndAlcoholTestId))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.TestDate, opt => opt.MapFrom(src => src.TestDate))
                .ForMember(dest => dest.TestType, opt => opt.MapFrom(src => src.TestType))
                .ForMember(dest => dest.TestResult, opt => opt.MapFrom(src => src.TestResult))
                .ForMember(dest => dest.TestDetails, opt => opt.MapFrom(src => src.TestDetails))
                .ForMember(dest => dest.IncidentReportId, opt => opt.MapFrom(src => src.IncidentReportId))
                .ForMember(dest => dest.FollowUpTestDate, opt => opt.MapFrom(src => src.FollowUpTestDate))
                .ForMember(dest => dest.IsFollowUpComplete, opt => opt.MapFrom(src => src.IsFollowUpComplete));
            CreateMap<DrugTestCreateDto, DrugAndAlcoholTest>().ReverseMap();
            CreateMap<DrugTestUpdateDto, DrugAndAlcoholTest>().ReverseMap();

            // Employee mappings
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Driver, EmployeeDto>()
                .IncludeBase<Employee, EmployeeDto>();

            CreateMap<Driver, DriverDto>().ReverseMap();
            CreateMap<UpdatePayRateDto, Employee>().ReverseMap();

            // Equipment mappings
            CreateMap<Vehicle, VehicleDto>().ReverseMap();
            CreateMap<PartDetails, PartDetailDto>().ReverseMap();

            // Financial mappings
            CreateMap<Payroll, PayrollDto>().ReverseMap();
            CreateMap<PurchaseOrder, PurchaseOrderDto>().ReverseMap();
            CreateMap<TimeSheet, TimesheetDto>().ReverseMap();

            // General mappings


            // Incident mappings
            CreateMap<IncidentReport, IncidentReportDto>().ReverseMap();

            // Misc mappings


            // Report mappings

            // Inventory mappings
            CreateMap<Inventory, InventoryDto>().ReverseMap();
            CreateMap<Inventory, InventoryCreateDto>().ReverseMap();
            CreateMap<Inventory, InventoryUpdateDto>().ReverseMap();
            CreateMap<Product, InventoryProductDetailDto>()
                .ForMember(dest => dest.QuantityOnHand, opt => opt.Ignore());

            CreateMap<Inventory, InventoryProductDetailDto>()
                .ForMember(dest => dest.QuantityOnHand, opt => opt.MapFrom(src => src.QuantityOnHand))
                // Assume ProductId matches and is already correctly set
                .AfterMap((src, dest, ctx) => {
                    var productDto = ctx.Mapper.Map<InventoryProductDetailDto>(src.Product);
                    dest.Name = productDto.Name;
                    dest.Description = productDto.Description;
                    dest.Price = productDto.Price;
                });
            CreateMap<InventoryProductDetailDto, Inventory>()
                .ForMember(dest => dest.QuantityOnHand, opt => opt.MapFrom(src => src.QuantityOnHand))
                .ForMember(dest => dest.Product, opt => opt.Ignore());


            // Product mappings
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, InventoryProductDetailDto>().ReverseMap();


            // Shipment mappings
            CreateMap<Shipment, IncomingShipmentDto>().ReverseMap();
            CreateMap<Shipment, OutgoingShipmentDto>().ReverseMap();
            CreateMap<Shipment, ShipmentCreateDto>().ReverseMap();
            CreateMap<Shipment, ShipmentDetailDto>().ReverseMap();
            CreateMap<Shipment, ShipmentUpdateDto>().ReverseMap();
            CreateMap<Manifest, ManifestDto>().ReverseMap();
            CreateMap<ManifestItem, ManifestItemDto>().ReverseMap();

            // User mappings
            CreateMap<UserAccount, UserAccountDto>().ReverseMap();

            CreateMap<PurchaseOrder, PurchaseOrderDto>()
                .ForMember(dest => dest.Manifest, opt => opt.MapFrom(src => src.Manifest))
                .ForMember(dest => dest.Shipments, opt => opt.MapFrom(src => src.Shipments))
                .ReverseMap();

            CreateMap<Manifest, ManifestDto>()
                .ForMember(dest => dest.ManifestItems, opt => opt.MapFrom(src => src.ManifestItems))
                .ReverseMap();

            CreateMap<ManifestItem, ManifestItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ReverseMap();

            CreateMap<Shipment, ShipmentDetailDto>()
                .ForMember(dest => dest.ManifestId, opt => opt.MapFrom(src => src.ManifestId))
                .ReverseMap();

            CreateMap<Product, ProductDto>()
                .ReverseMap();

            CreateMap<Shipment, ShipmentDetailDto>()
                .ForMember(dest => dest.Manifest, opt => opt.MapFrom(src => src.Manifest));

            CreateMap<Manifest, ShipmentManifestDto>();
            CreateMap<ManifestItem, ManifestItemDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));  // Assuming you have a relationship with the Product entity

            CreateMap<MaintenanceRecord, MaintenanceRecordDto>()
                .ForMember(dest => dest.MaintenanceRecordId, opt => opt.MapFrom(src => src.MaintenanceRecordId))
                .ForMember(dest => dest.RecordType, opt => opt.MapFrom(src => src.MaintenanceRecordId))
                .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId.HasValue ? src.VehicleId.Value : default(int)))
                .ForMember(dest => dest.MaintenanceDate, opt => opt.MapFrom(src => src.MaintenanceDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.PartIds, opt => opt.Ignore()); // Ignoring the PartIds as they are not needed

            CreateMap<Shipment, ShipmentDetailDto>()
                .ForMember(dest => dest.ShipmentId, opt => opt.MapFrom(src => src.ShipmentId))
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction))
                .ForMember(dest => dest.hasArrived, opt => opt.MapFrom(src => src.hasArrived))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.Zip))
                .ForMember(dest => dest.VehicleId, opt => opt.MapFrom(src => src.VehicleId))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.ManifestId, opt => opt.MapFrom(src => src.ManifestId))
                .ForMember(dest => dest.Manifest,
                    opt => opt.MapFrom(src =>
                        src.Manifest)) // Assuming you have a mapping for Manifest to ShipmentManifestDto
                .ForMember(dest => dest.PurchaseOrderId, opt => opt.MapFrom(src => src.PurchaseOrderId))
                .ForMember(dest => dest.DepDateTime, opt => opt.MapFrom(src => src.DepDateTime))
                .ForMember(dest => dest.EstimatedArrivalTime, opt => opt.MapFrom(src => src.EstimatedArrivalTime))
                .ForMember(dest => dest.ActualArrivalTime, opt => opt.MapFrom(src => src.ActualArrivalTime))
                .ForMember(dest => dest.TotalCost,
                    opt => opt.Ignore()) // Assuming TotalCost is a property of Manifest
                .ForMember(dest => dest.IsFullyPaid, opt => opt.Ignore());

            CreateMap<IncidentReportDto, DrugAndAlcoholTest>().ReverseMap();

            CreateMap<VehicleCreateDto, Vehicle>()
                .ForMember(dest => dest.VehicleId, opt => opt.Ignore())
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));


        }
    }
}
