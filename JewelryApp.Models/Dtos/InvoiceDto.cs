using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos;

public class InvoiceDto
{
    public InvoiceDto()
    {
        Products = new List<ProductDto>();
    }

    public int Id { get; set; }

    [Display(Name = "نام خریدار")]
    public string BuyerFirstName { get; set; }

    [Display(Name = "نام خانوادگی خریدار")]
    public string BuyerLastName { get; set; }

    [Display(Name = "کد ملی خریدار")]
    public string BuyerNationalCode { get; set; }

    [Display(Name = "شماره همراه خریدار")]
    public string BuyerPhoneNumber { get; set; }

    public List<ProductDto> Products { get; set; }

    [Display(Name = "تاریخ فاکتور")]
    public DateTime? BuyDateTime { get; set; }

    [Display(Name = "نرخ گرم 18 عیار")]
    public double GramPrice { get; set; }

    [JsonIgnore]
    public double TotalPrice
    {
        get
        {
            if (Products != null)
                return Products.Sum(x => x.FinalPrice) + Products.Sum(x => x.Tax);

            return 0;
        }
    }

    [JsonIgnore]
    public double TotalTax
    {
        get
        {
            if (Products != null)
                return Products.Sum(x => x.Tax);

            return 0;
        }
    }

    [JsonIgnore]
    public double TotalFinalPrice
    {
        get
        {
            if (Products != null)
                return Products.Sum(x => x.FinalPrice) - Discount;

            return 0;
        }
    }

    [Display(Name = "تخفیف")]
    public double Discount { get; set; } = 0;

    [Display(Name = "بدهی")]
    public double Debt { get; set; } = 0;

    [Display(Name = "موعد بدهی")]
    public DateTime? DebtDate { get; set; }
}
