using AutoMapper;
using JewelryApp.Common.DateFunctions;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.Authentication;
using JewelryApp.Models.Dtos.Common;
using JewelryApp.Models.Dtos.Invoice;
using JewelryApp.Models.Dtos.Product;

namespace JewelryApp.Business.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Product, ProductTableItemDto>().ReverseMap();
        CreateMap<Price, PriceDto>().ReverseMap();
        CreateMap<Invoice, InvoiceDto>().ReverseMap();
        CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
        CreateMap<Product, InvoiceItemDto>().ReverseMap();
        CreateMap<InvoiceDto, InvoiceTableItemDto>()
            .ForMember(x => x.CustomerName,
                a =>
                    a.MapFrom(x => x.Customer.Name))
            .ForMember(x => x.BuyDate, 
                x => x.MapFrom(a => a.BuyDateTime.ToShamsiDateString()))
            .ForMember(x => x.CustomerPhone, 
                a => a.MapFrom(x => x.Customer.Phone))
            .ForMember(x => x.ProductsCount, 
                a => a.MapFrom(x => x.Products.Count))
            .ForMember(x => x.TotalCost,
                a =>
                    a.MapFrom(x => x.Products.Sum(y => y.FinalPrice)))
            .ForMember(x => x.InvoiceId, a =>
                a.MapFrom(b => b.Id));
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateProjection<RefreshToken, RefreshTokenDto>();
    }
}
