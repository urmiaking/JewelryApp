using System.ComponentModel.DataAnnotations;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.CommonDtos;

namespace JewelryApp.Models.Dtos.InvoiceDtos;

public class CustomerDto : BaseDto<CustomerDto, Customer>
{
    [Display(Name = "نام و نام خانوادگی مشتری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string FullName { get; set; }

    [Display(Name = "شماره همراه")]
    public string PhoneNumber { get; set; }
}