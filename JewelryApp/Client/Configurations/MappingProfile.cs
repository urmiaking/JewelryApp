using AutoMapper;
using JewelryApp.Client.ViewModels;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Invoices;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Client.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginVm, AuthenticationRequest>();
        CreateMap<GetInvoiceListResponse, InvoicesListVm>();
        CreateMap<InvoicesListVm, UpdateInvoiceRequest>();
        CreateMap<AddProductVm, AddProductRequest>();
        CreateMap<GetProductResponse, ProductListVm>();
        CreateMap<ChangePasswordVm, ChangePasswordRequest>();
        CreateMap<GetProductResponse, CalculatorVm>();
    }
}
