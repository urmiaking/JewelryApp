using AutoMapper;
using JewelryApp.Application.ExternalModels.Signal;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.Enums;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Requests.ProductCategories;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Customer;
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

        CreateMap<Product, AddProductResponse>()
            .ConstructUsing(x => new AddProductResponse(x.Id, x.Name, x.Weight, x.Wage, (int)x.WageType,
                (int)x.ProductType, (int)x.Carat, x.ProductCategoryId, x.Barcode));

        CreateMap<UpdateProductRequest, Product>()
            .ForMember(x => x.Carat, a => a.MapFrom(b => b.CaratType))
            .ForMember(x => x.ProductCategoryId, a => a.MapFrom(b => b.CategoryId));

        CreateMap<Product, UpdateProductResponse>()
            .ConstructUsing(x => new UpdateProductResponse(x.Id, x.Name, x.Weight, x.Wage, (int)x.WageType,
                (int)x.ProductType, (int)x.Carat, x.ProductCategoryId, x.Barcode));

        CreateProjection<Product, GetProductResponse>()
            .ForMember(x => x.CaratType, a => a.MapFrom(b => b.Carat.ToDisplay()))
            .ForMember(x => x.ProductType, a => a.MapFrom(b => b.ProductType.ToDisplay()))
            .ForMember(x => x.WageType, a => a.MapFrom(b => b.WageType.ToDisplay()))
            .ForMember(x => x.CategoryName, a => a.MapFrom(b => b.ProductCategory.Name));

        CreateMap<Product, GetProductResponse>()
            .ConstructUsing(x => new GetProductResponse(x.Id, x.Name, x.Weight, x.Wage, x.WageType.ToDisplay(),
                x.ProductType.ToDisplay(), x.Carat.ToDisplay(), x.ProductCategory.Name, x.Barcode, x.Deleted));

        #endregion

        #region Price

        CreateMap<PriceApiResult, Price>().ReverseMap();
        CreateMap<PriceResponse, Price>().ReverseMap();

        #endregion

        #region Invoice

        CreateProjection<Invoice, GetInvoiceListResponse>()
            .ConstructUsing(x => new GetInvoiceListResponse(x.Id, x.InvoiceNumber, x.Customer.FullName,
                x.Customer.PhoneNumber,
                x.CalculateTotalPrice(), x.InvoiceItems.Count, x.InvoiceDate, x.Deleted));
        CreateMap<Invoice, GetInvoiceResponse>()
            .ConstructUsing(x => new GetInvoiceResponse(x.Id, x.InvoiceNumber, x.CustomerId, x.InvoiceDate, x.Discount,
                x.AdditionalPrices, x.Difference, x.Debt, x.DebtDate, x.InvoiceItems.Sum(a => a.Price),
                x.InvoiceItems.Sum(a => a.Tax), x.CalculateTotalPrice()));

        CreateMap<AddInvoiceRequest, Invoice>();

        CreateMap<Invoice, AddInvoiceResponse>();

        CreateMap<UpdateInvoiceRequest, Invoice>();

        CreateMap<Invoice, UpdateInvoiceResponse>()
            .ConstructUsing(x => new UpdateInvoiceResponse(x.Id, x.InvoiceNumber, x.CustomerId, x.InvoiceDate, x.Discount, x.AdditionalPrices, x.Difference, x.Debt, x.DebtDate));

        #endregion

        #region Customer

        CreateMap<AddCustomerRequest, Customer>()
            .ForMember(x => x.FullName, a => a.MapFrom(b => b.Name));

        CreateMap<Customer, AddCustomerResponse>()
            .ConstructUsing(x => new AddCustomerResponse(x.Id, x.FullName, x.PhoneNumber));

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
