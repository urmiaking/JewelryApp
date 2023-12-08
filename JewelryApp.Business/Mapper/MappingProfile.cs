using AutoMapper;
using JewelryApp.Business.ExternalModels.Signal;
using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Responses.Authentication;
using JewelryApp.Shared.Responses.Products;

namespace JewelryApp.Business.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        CreateMap<PriceApiResult, Price>().ReverseMap();
        CreateMap<PriceResponse, Price>().ReverseMap();
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
    }
}
