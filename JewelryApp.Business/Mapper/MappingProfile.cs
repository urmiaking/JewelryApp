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
        CreateMap<Invoice, InvoiceDto>().ReverseMap();
        CreateMap<ProductDto, InvoiceProduct>().ForMember(x => x.ProductId,
            a => 
                a.MapFrom(b => b.Id)).ReverseMap();
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateProjection<RefreshToken, RefreshTokenDto>();
    }
}
