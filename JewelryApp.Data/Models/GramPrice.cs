namespace JewelryApp.Data.Models;

public class GramPrice
{
    public int Id { get; set; }
    public DateTime RequestDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public double Price { get; set; }
    public double Change { get; set; }
    public string UpdatedDateTimeString { get; set; }
}