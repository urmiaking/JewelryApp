using AutoMapper;
using JewelryApp.Common.DateFunctions;
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
        CreateMap<Invoice, InvoiceTableItemDto>()
            .ForMember(x => x.BuyerName,
                a =>
                    a.MapFrom(x => $"{x.BuyerFirstName} {x.BuyerLastName}"))
            .ForMember(x => x.BuyDate, x =>
                x.MapFrom(a => a.BuyDateTime.Value.ToShamsiDateString()))
            .ForMember(x => x.BuyerPhone, a => a.MapFrom(x => x.BuyerPhoneNumber))
            .ForMember(x => x.ProductsCount, a =>
                a.MapFrom(x => x.InvoiceProducts.Count))
            .ForMember(x => x.TotalCost,
                a =>
                    a.MapFrom(x => x.InvoiceProducts.Sum(y => y.FinalPrice)))
            .ForMember(x => x.InvoiceId, a =>
                a.MapFrom(b => b.Id));
        CreateProjection<RefreshToken, RefreshTokenDto>();
    }
}
