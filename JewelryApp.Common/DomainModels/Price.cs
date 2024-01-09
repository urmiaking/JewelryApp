namespace JewelryApp.Core.DomainModels;

public class Price : IEntity
{
    public int Id { get; set; }
    public double Gram17 { get; set; }
    public double Gram18 { get; set; }
    public double Gram24 { get; set; }
    public double Mazanneh { get; set; }
    public double Mesghal { get; set; }

    public double UsDollar { get; set; }
    public double UsEuro { get; set; }

    public double CoinImam { get; set; }
    public double CoinNim { get; set; }
    public double CoinRob { get; set; }
    public double CoinBahar { get; set; }
    public double CoinGrami { get; set; }
    public double CoinParsian500Sowt { get; set; }
    public double CoinParsian400Sowt { get; set; }
    public double CoinParsian300Sowt { get; set; }
    public double CoinParsian250Sowt { get; set; }
    public double CoinParsian200Sowt { get; set; }
    public double CoinParsian150Sowt { get; set; }
    public double CoinParsian100Sowt { get; set; }
    public double CoinParsian50Sowt { get; set; }
    public double CoinParsian1Gram { get; set; }
    public double CoinParsian2Gram { get; set; }
    public double CoinParsian15Gram { get; set; }

    public string DateTime { get; set; } = default!;
}

