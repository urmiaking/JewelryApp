using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Models.Dtos.Invoice;

public class CustomerDto
{
    public int Id { get; set; }

    [Display(Name = "نام و نام خانوادگی مشتری")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Name { get; set; }

    [Display(Name = "شماره همراه")]
    public string Phone { get; set; }
}