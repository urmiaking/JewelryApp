using JewelryApp.Models.AppModels;

namespace JewelryApp.Client.Services;

public interface IPriceService
{
    public Task<PriceModel?> GetPrice();
}