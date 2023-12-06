using AutoMapper;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.InvoiceDtos;

namespace JewelryApp.Business.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
        CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
    }
}
