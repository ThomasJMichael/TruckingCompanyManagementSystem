using AutoMapper;
using TCMS.Common.DTOs.Inventory;
using TCMS.GUI.Models;
using TCMS.GUI.ViewModels;

namespace TCMS.GUI.Utilities
{
    public class GUIAutoMapperProfile : Profile
    {
        public GUIAutoMapperProfile()
        {
            // Products
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductFormViewModel, ProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price)); 

            CreateMap<InventoryProductDetailDto, Product>().ReverseMap();
        }
    };
}
