using AutoMapper;
using JewelryApp.Business.ExternalModels.Signal;
using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Authentication;
using JewelryApp.Shared.Responses.Invoices;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Business.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        #region Product

        CreateMap<AddProductRequest, Product>().ReverseMap()
            .ForMember(x => x.CaratType, a => a.MapFrom(b => b.Carat))
            .ForMember(x => x.CategoryId, a => a.MapFrom(b => b.ProductCategoryId));
        CreateMap<AddProductResponse, Product>().ReverseMap()
            .ForMember(x => x.CaratType, a => a.MapFrom(b => b.Carat))
            .ForMember(x => x.CategoryId, a => a.MapFrom(b => b.ProductCategoryId));
        CreateMap<UpdateProductResponse, Product>().ReverseMap()
            .ForMember(x => x.CaratType, a => a.MapFrom(b => b.Carat))
            .ForMember(x => x.CategoryId, a => a.MapFrom(b => b.ProductCategoryId));
        CreateMap<UpdateProductResponse, Product>().ReverseMap()
            .ForMember(x => x.CaratType, a => a.MapFrom(b => b.Carat))
            .ForMember(x => x.CategoryId, a => a.MapFrom(b => b.ProductCategoryId)); ;
        CreateProjection<Product, GetProductResponse>()
            .ForMember(x => x.CaratType, a => a.MapFrom(b => b.Carat.ToDisplay()))
            .ForMember(x => x.CategoryName, a => a.MapFrom(b => b.ProductCategory.Name));
        CreateMap<GetProductResponse, Product>().ReverseMap()
            .ForMember(x => x.CaratType, a => a.MapFrom(b => b.Carat.ToDisplay()))
            .ForMember(x => x.CategoryName, a => a.MapFrom(b => b.ProductCategory.Name));

        #endregion

        #region Price

        CreateMap<PriceApiResult, Price>().ReverseMap();
        CreateMap<PriceResponse, Price>().ReverseMap();

        #endregion

        #region Invoice

        CreateProjection<Invoice, GetInvoiceTableResponse>()
            .ForMember(i => i.InvoiceItemsCount, a => a.MapFrom(b => b.InvoiceItems.Count))
            .ForMember(i => i.CustomerPhoneNumber, a => a.MapFrom(b => b.Customer.PhoneNumber))
            .ForMember(i => i.CustomerName, a => a.MapFrom(b => b.Customer.FullName));

        CreateMap<GetInvoiceDetailsResponse, Invoice>().ReverseMap()
            .ForMember(x => x.CustomerName, a => a.MapFrom(b => b.Customer.FullName))
            .ForMember(x => x.CustomerPhoneNumber, a => a.MapFrom(b => b.Customer.PhoneNumber))
            .ForMember(x => x.CustomerId, a => a.MapFrom(b => b.Customer.Id))
            .ForMember(x => x.TotalRawPrice, a => a.MapFrom(b => b.InvoiceItems.Sum(x => x.Price)))
            .ForMember(x => x.TotalTax, a => a.MapFrom(b => b.InvoiceItems.Sum(x => x.Tax)))
            .ForMember(x => x.TotalFinalPrice, a => a.MapFrom(b => b.InvoiceItems.Sum(x => x.Tax) + b.InvoiceItems.Sum(x => x.Price)));

        #endregion
    }
}
