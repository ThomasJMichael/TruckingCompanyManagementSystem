using AutoMapper;
using TCMS.Common.DTOs.Employee;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.DTOs.Inventory;
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

        }

    };
}
