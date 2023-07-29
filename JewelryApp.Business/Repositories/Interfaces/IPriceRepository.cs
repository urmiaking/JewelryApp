namespace JewelryApp.Business.Repositories.Interfaces;

public interface IPriceRepository
{
    Task<double> GetGramPrice();
}