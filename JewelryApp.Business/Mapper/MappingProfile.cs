using AutoMapper;
using JewelryApp.Application.ExternalModels.Signal;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Enums;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Requests.ProductCategories;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Invoices;
using JewelryApp.Shared.Responses.Prices;
using JewelryApp.Shared.Responses.ProductCategories;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Application.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        #region Product

        CreateMap<AddProductRequest, Product>()
            .ForMember(x => x.Carat, a => a.MapFrom(b => b.CaratType))
            .ForMember(x => x.ProductCategoryId, a => a.MapFrom(b => b.CategoryId));
        CreateMap<Product, AddProductResponse>().ConstructUsing(x => new(x.Id, x.Name, x.Weight, x.Wage, (int)x.WageType, (int)x.ProductType, (int)x.Carat, x.ProductCategoryId, x.Barcode))
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

        CreateMap<GetInvoiceResponse, Invoice>().ReverseMap()
            .ForMember(x => x.CustomerId, a => a.MapFrom(b => b.Customer.Id))
            .ForMember(x => x.TotalRawPrice, a => a.MapFrom(b => b.InvoiceItems.Sum(x => x.Price)))
            .ForMember(x => x.TotalTax, a => a.MapFrom(b => b.InvoiceItems.Sum(x => x.Tax)))
            .ForMember(x => x.TotalFinalPrice, a => a.MapFrom(b => b.InvoiceItems.Sum(x => x.Tax) + b.InvoiceItems.Sum(x => x.Price)));

        #endregion

        #region Customer

        CreateMap<AddCustomerRequest, Customer>();

        #endregion

        #region Product Category

        CreateProjection<ProductCategory, GetProductCategoryResponse>();
        CreateMap<ProductCategory, GetProductCategoryResponse>();
        CreateMap<AddProductCategoryRequest, ProductCategory>();
        CreateMap<ProductCategory, AddProductCategoryResponse>();
        CreateMap<UpdateProductCategoryRequest, ProductCategory>();
        CreateMap<ProductCategory, UpdateProductCategoryResponse>();

        #endregion
    }
}
