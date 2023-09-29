using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace JewelryApp.Models.Dtos.Invoice;

public class InvoiceDto
{
    public int Id { get; set; }

    [Display(Name = "تاریخ فاکتور")]
    public DateTime BuyDateTime { get; set; }

    [Display(Name = "نرخ گرم روز")]
    public double GramPrice { get; set; }

    [Display(Name = "تخفیف")]
    public double Discount { get; set; } = 0;

    [Display(Name = "بدهی")]
    public double Debt { get; set; } = 0;

    [Display(Name = "موعد بدهی")]
    public DateTime? DebtDate { get; set; }

    public CustomerDto Customer { get; set; }
    public List<InvoiceItemDto> Products { get; set; }

    [JsonIgnore]
    public double TotalRawPrice
    {
        get
        {
            if (Products != null)
                return Products.Sum(x => x.FinalPrice);

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
                return TotalRawPrice + TotalTax - Discount - Debt;

            return 0;
        }
    }
}
