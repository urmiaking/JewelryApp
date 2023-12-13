﻿namespace JewelryApp.Core.DomainModels;

public class Price : IEntity
{
    public int Id { get; set; }
    public double Gram17 { get; set; }
    public double Gram18 { get; set; }
    public double Gram24 { get; set; }
    public double Mazanneh { get; set; }
    public double Mesghal { get; set; }
    public double UsDollar { get; set; }

    public DateTime? DateTime { get; set; }
}

