using AutoMapper;
using TCMS.Common.DTOs.Incident;
using TCMS.Common.DTOs.Inventory;
using TCMS.GUI.Models;
using TCMS.GUI.ViewModels;
using TCMS.Common.DTOs.Inventory;
using TCMS.Common.DTOs.Incident;
namespace TCMS.GUI.Utilities
{

    public class GUIAutoMapperProfile : Profile
    {
        public GUIAutoMapperProfile()
        {
            // Products
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductFormViewModel, InventoryProductDetailDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.QuantityOnHand, opt => opt.MapFrom(src => src.QuantityOnHand));
            CreateMap<IncidentReportDto, IncidentReport>().ReverseMap();
            CreateMap<IncidentLogFormViewModel, IncidentReportDto>()
                .ForMember(dest => dest.IncidentReportId, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IncidentDate, opt => opt.MapFrom(src => src.Price));

            CreateMap<InventoryProductDetailDto, Product>().ReverseMap();
            CreateMap<AddProductDto, ProductFormViewModel>().ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.InitialQuantityOnHand, opt => opt.MapFrom(src => src.QuantityOnHand));
        }

    };
}
