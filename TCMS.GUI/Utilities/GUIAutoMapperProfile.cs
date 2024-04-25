using AutoMapper;
using TCMS.Common.DTOs.DrugTest;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.Financial;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.DTOs.Shipment;
using TCMS.Common.DTOs.User;
using TCMS.GUI.Models;
using TCMS.GUI.ViewModels;

namespace TCMS.GUI.Utilities
{

    public class GUIAutoMapperProfile : Profile
    {
        public GUIAutoMapperProfile()
        {
            // Inventory Mapping
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductFormViewModel, InventoryProductDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.QuantityOnHand, opt => opt.MapFrom(src => src.QuantityOnHand));

            CreateMap<InventoryProductDetailDto, Product>().ReverseMap();
            CreateMap<AddProductDto, ProductFormViewModel>().ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.InitialQuantityOnHand, opt => opt.MapFrom(src => src.QuantityOnHand));

            // Incident Mapping
            CreateMap<IncidentReportDto, IncidentReport>().ReverseMap();
            CreateMap<IncidentLogFormViewModel, IncidentReportDto>()
                .ForMember(dest => dest.IncidentReportId, opt => opt.MapFrom(src => src.IncidentReportId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IncidentDate, opt => opt.MapFrom(src => src.SelectedDate));
            CreateMap<DrugTestFormViewModel, DrugTestCreateDto>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.TestType, opt => opt.MapFrom(src => src.TestType))
                .ForMember(dest => dest.TestResult, opt => opt.MapFrom(src => src.TestResult))
                .ForMember(dest => dest.TestDate, opt => opt.MapFrom(src => src.TestDate))
                .ForMember(dest => dest.TestDetails, opt => opt.MapFrom(src => src.TestDetails))
                .ForMember(dest => dest.IncidentReportId, opt => opt.MapFrom(src => src.IncidentReportId))
                .ForMember(dest => dest.FollowUpTestDate, opt => opt.MapFrom(src => src.FollowUpTestDate));



            // Employee Mapping
            CreateMap<EmployeeDto, Employee>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.Zip))
                .ForMember(dest => dest.HomePhoneNumber, opt => opt.MapFrom(src => src.HomePhoneNumber))
                .ForMember(dest => dest.CellPhoneNumber, opt => opt.MapFrom(src => src.CellPhoneNumber))
                .ForMember(dest => dest.PayRate, opt => opt.MapFrom(src => src.PayRate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole))
                .ForMember(dest => dest.CDLNumber, opt => opt.MapFrom(src => src.CDLNumber))
                .ForMember(dest => dest.CDLExperationDate, opt => opt.MapFrom(src => src.CDLExperationDate))
                .ReverseMap();
            CreateMap<UserAccountDto, Employee>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));

            // EmployeeForm to EmployeeDto
            CreateMap<EmployeeFormViewModel, EmployeeDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.Zip))
                .ForMember(dest => dest.HomePhoneNumber, opt => opt.MapFrom(src => src.HomePhoneNumber))
                .ForMember(dest => dest.CellPhoneNumber, opt => opt.MapFrom(src => src.CellPhoneNumber))
                .ForMember(dest => dest.PayRate, opt => opt.MapFrom(src => src.PayRate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));
            CreateMap<EmployeeFormViewModel, UserRoleDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRole));

            CreateMap<ManifestDto, Manifest>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ManifestItems))
                .ReverseMap();

            // Mapping for PurchaseOrderDto to PurchaseOrder
            CreateMap<PurchaseOrderDto, PurchaseOrder>()
                .ForMember(dest => dest.Manifest, opt => opt.MapFrom(src => src.Manifest))
                .ForMember(dest => dest.Shipments, opt => opt.MapFrom(src => src.Shipments))
                .ForMember(dest => dest.TotalItemCost,
                    opt => opt.Ignore()) // Will be calculated in CalculateTotals method
                .ForMember(dest => dest.TotalCost, opt => opt.Ignore()); // Will be calculated in CalculateTotals method

            // Mapping for ManifestDto to Manifest
            CreateMap<ManifestDto, Manifest>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ManifestItems));

            // Mapping for ManifestItemDto to ManifestItem
            CreateMap<ManifestItemDto, ManifestItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ItemStatus, opt => opt.MapFrom(src => src.ItemStatus));

            // Mapping for ProductDto to Product
            CreateMap<ProductDto, Product>();

            // Mapping for ShipmentDetailDto to Shipment
            CreateMap<ShipmentDetailDto, Shipment>();

            CreateMap<ShipmentDetailDto, Shipment>()
                .ForMember(dest => dest.IsArrived, opt => opt.MapFrom(src => src.hasArrived))
                .ForMember(dest => dest.ManifestId, opt => opt.MapFrom(src => src.Manifest.ManifestId));

            CreateMap<ShipmentManifestDto, Manifest>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ManifestItems));

            CreateMap<ManifestItemDto, ManifestItem>()
                .ForMember(dest => dest.ItemStatus, opt => opt.MapFrom(src => src.ItemStatus));


        }
    };
}
