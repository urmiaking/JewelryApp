using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JewelryApp.Data.Models;

public class Price
{
    public int Id { get; set; }
    public double Gold18K { get; set; }
    public double Gold24K { get; set; }
    public double GoldOunce { get; set; }
    public double OldCoin { get; set; }
    public double NewCoin { get; set; }
    public double HalfCoin { get; set; }
    public double QuarterCoin { get; set; }
    public double GramCoin { get; set; }

    public DateTime? DateTime { get; set; }
}

