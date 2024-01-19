using AutoMapper;
using JewelryApp.Client.ViewModels;
using JewelryApp.Client.ViewModels.Invoice;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Requests.ProductCategories;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Invoices;
using JewelryApp.Shared.Responses.ProductCategories;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Client.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginVm, AuthenticationRequest>();
        CreateMap<GetInvoiceListResponse, InvoicesListVm>();
        CreateMap<InvoicesListVm, UpdateInvoiceRequest>();
        CreateMap<AddProductVm, AddProductRequest>()
            .ConstructUsing(x => new AddProductRequest(x.Name, x.Weight, x.Wage, x.WageType.ToString(), x.ProductType.ToString(), x.CaratType.ToString(),
                                        x.ProductCategory.Id, x.Barcode));
        CreateMap<GetProductResponse, ProductListVm>();
        CreateMap<ChangePasswordVm, ChangePasswordRequest>();
        CreateMap<GetProductResponse, CalculatorVm>();
        CreateMap<GetProductCategoryResponse, ProductCategoryVm>();

        CreateMap<ProductCategoryVm, UpdateProductCategoryRequest>();
        CreateMap<ProductCategoryVm, AddProductCategoryRequest>();

        CreateMap<GetProductResponse, AddInvoiceItemVm>();
        CreateMap<AddProductResponse, AddInvoiceItemVm>();
        CreateMap<GetProductResponse, AddProductVm>();
        CreateMap<GetProductResponse, EditInvoiceItemVm>();
        CreateMap<AddInvoiceItemVm, EditInvoiceItemVm>();
        CreateMap<EditInvoiceItemVm, AddInvoiceItemVm>();
        CreateMap<EditInvoiceItemVm, UpdateProductRequest>()
            .ConstructUsing(x => new UpdateProductRequest(x.Id, x.Name, x.Weight, x.Wage, x.WageType.ToString(), x.ProductType.ToString(), x.CaratType.ToString(),
            x.ProductCategory.Id, x.Barcode));
    }
}
