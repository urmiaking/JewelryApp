using AutoMapper;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, SetProductDto>().ReverseMap();
        CreateMap<ProductDto, InvoiceProduct>().ReverseMap();
        CreateMap<Product, ProductTableItemDto>().ReverseMap();
        CreateMap<Price, PriceDto>().ReverseMap();
        CreateProjection<RefreshToken, RefreshTokenDto>();
    }
}
