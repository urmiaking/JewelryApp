using AutoMapper;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        CreateMap<Product, SetProductDto>().ReverseMap();
        CreateMap<Product, ProductTableItemDto>().ReverseMap();
        CreateMap<Price, PriceDto>().ReverseMap();
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(x => 
                x.Products, x => 
                x.MapFrom(a => a.InvoiceProducts))
            .ForMember(x => 
                x.GramPrice, x =>
                x.MapFrom(a => a.InvoiceProducts.FirstOrDefault().GramPrice))
            .ReverseMap();

        CreateMap<ProductDto, InvoiceProduct>().ForMember(x => x.ProductId,
            a => a.MapFrom(b => b.Id)).ReverseMap();

        CreateMap<InvoiceProduct, ProductDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Product.Weight))
            .ForMember(dest => dest.Wage, opt => opt.MapFrom(src => src.Product.Wage))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Product.Id));

        CreateMap<ProductDto, Product>().ReverseMap();
        CreateProjection<RefreshToken, RefreshTokenDto>();
    }
}
