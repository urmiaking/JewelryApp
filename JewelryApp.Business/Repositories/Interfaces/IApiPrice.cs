using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.Repositories.Interfaces;

public interface IApiPrice
{
    Task<double> GetGramPrice();
}