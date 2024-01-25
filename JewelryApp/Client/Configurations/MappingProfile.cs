using AutoMapper;
using JewelryApp.Client.ViewModels;
using JewelryApp.Client.ViewModels.Invoice;
using JewelryApp.Client.ViewModels.Product;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Shared.Requests.InvoiceItems;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Requests.OldGolds;
using JewelryApp.Shared.Requests.ProductCategories;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Customer;
using JewelryApp.Shared.Responses.InvoiceItems;
using JewelryApp.Shared.Responses.Invoices;
using JewelryApp.Shared.Responses.OldGolds;
using JewelryApp.Shared.Responses.ProductCategories;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Client.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Authentication

        CreateMap<LoginVm, AuthenticationRequest>();
        CreateMap<ChangePasswordVm, ChangePasswordRequest>();

        #endregion

        #region Calculator

        CreateMap<GetProductResponse, CalculatorVm>();

        #endregion

        #region Invoices

        CreateMap<GetInvoiceListResponse, InvoicesListVm>();
        CreateMap<InvoicesListVm, UpdateInvoiceRequest>();
        CreateMap<AddInvoiceVm, AddInvoiceRequest>()
            .ConstructUsing(x => new AddInvoiceRequest(x.InvoiceNumber, x.BuyDateTime!.Value, x.Debt, x.DebtDate,
                x.AdditionalPrices, x.Discount, default));
        CreateMap<GetInvoiceResponse, ViewInvoiceVm>();

        #endregion

        #region InvoiceItems

        CreateMap<GetProductResponse, AddInvoiceItemVm>()
            .ForMember(x => x.ProductId, a => a.MapFrom(x => x.Id))
            .ForMember(x => x.Id, a => a.DoNotUseDestinationValue());

        CreateMap<AddProductResponse, AddInvoiceItemVm>()
            .ForMember(x => x.ProductId, a => a.MapFrom(x => x.Id))
            .ForMember(x => x.Id, a => a.DoNotUseDestinationValue());

        CreateMap<GetProductResponse, EditInvoiceItemVm>()
            .ForMember(x => x.ProductId, a => a.MapFrom(x => x.Id))
            .ForMember(x => x.Id, a => a.DoNotUseDestinationValue());

        CreateMap<AddInvoiceItemVm, EditInvoiceItemVm>();
        CreateMap<EditInvoiceItemVm, AddInvoiceItemVm>();
        CreateMap<EditInvoiceItemVm, UpdateProductRequest>()
            .ConstructUsing(x => new UpdateProductRequest(x.Id, x.Name, x.Weight, x.Wage, x.WageType.ToString(), x.ProductType.ToString(), x.CaratType.ToString(),
                x.ProductCategory.Id, x.Barcode));
        CreateMap<AddInvoiceItemVm, AddInvoiceItemRequest>()
            .ConstructUsing(x => new AddInvoiceItemRequest(x.InvoiceId, x.ProductId, x.Profit, x.GramPrice,
                x.DollarPrice, x.TaxOffset, x.Tax, x.FinalPrice));

        CreateMap<GetInvoiceItemResponse, ViewInvoiceItemVm>();

        #endregion

        #region Products

        CreateMap<AddProductVm, AddProductRequest>()
            .ConstructUsing(x => new AddProductRequest(x.Name, x.Weight, x.Wage, x.WageType.ToString(), x.ProductType.ToString(), x.CaratType.ToString(),
                x.ProductCategory.Id, x.Barcode));
        CreateMap<GetProductResponse, ProductListVm>();
        CreateMap<GetProductResponse, AddProductVm>();
        CreateMap<GetProductResponse, EditProductVm>();

        #endregion

        #region ProductCategories

        CreateMap<GetProductCategoryResponse, ProductCategoryVm>();
        CreateMap<ProductCategoryVm, UpdateProductCategoryRequest>();
        CreateMap<ProductCategoryVm, AddProductCategoryRequest>();

        #endregion

        #region Customer

        CreateMap<AddCustomerVm, AddCustomerRequest>();
        CreateMap<AddCustomerResponse, AddCustomerVm>();
        CreateMap<GetCustomerResponse, AddCustomerVm>();
        CreateMap<GetCustomerResponse, ViewCustomerVm>();

        #endregion

        #region OldGolds

        CreateMap<AddOldGoldVm, AddOldGoldRequest>()
            .ConstructUsing(x => new AddOldGoldRequest(x.Name, x.Weight, x.InvoiceId, x.Price, x.BuyDateTime));

        CreateMap<List<GetOldGoldResponse>, ViewOldGoldsVm>()
            .ForMember(x => x.TotalCount, a => a.MapFrom(x => x.Count))
            .ForMember(x => x.Price, a => a.MapFrom(x => x.Sum(b => b.Price)));

        #endregion
    }
}
