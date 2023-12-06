using AutoMapper;
using JewelryApp.Common.DateFunctions;
using JewelryApp.Models.Dtos.CommonDtos;

namespace JewelryApp.Models.Dtos.InvoiceDtos;

public class InvoiceTableItemDto : BaseDto<InvoiceTableItemDto, InvoiceDto>
{
    public int InvoiceId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public double TotalCost { get; set; }
    public int ProductsCount { get; set; }
    public string BuyDate { get; set; }

    public override void CustomMappings(IMappingExpression<InvoiceDto, InvoiceTableItemDto> mapping)
    {
        mapping.ForMember(x => x.CustomerName,
                a =>
                    a.MapFrom(x => x.Customer.FullName))
            .ForMember(x => x.BuyDate,
                x => x.MapFrom(a => a.BuyDateTime.ToShamsiDateString()))
            .ForMember(x => x.CustomerPhone,
                a => a.MapFrom(x => x.Customer.PhoneNumber))
            .ForMember(x => x.ProductsCount,
                a => a.MapFrom(x => x.Products.Count))
            .ForMember(x => x.TotalCost,
                a =>
                    a.MapFrom(x => x.Products.Sum(y => y.FinalPrice)))
            .ForMember(x => x.InvoiceId, a =>
                a.MapFrom(b => b.Id));
    }
}