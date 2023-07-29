using System.ComponentModel.DataAnnotations;

namespace JewelryApp.Data.Models;

public class ApiKey
{
    public int Id { get; set; }

    [Display(Name= "کلید API")]
    public string Key { get; set; }
    public DateTime AddDateTime { get; set; }
    public bool IsActive { get; set; }
}