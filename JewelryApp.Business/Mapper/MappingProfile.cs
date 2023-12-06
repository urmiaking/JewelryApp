using AutoMapper;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.InvoiceDtos;
using JewelryApp.Models.Dtos.PriceDtos.Signal;

namespace JewelryApp.Business.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
        CreateMap<PriceApiResult, Price>().ReverseMap();
    }
}
