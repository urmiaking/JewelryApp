namespace JewelryApp.Data.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    
    public virtual ICollection<Invoice> Invoices { get; set; }
}