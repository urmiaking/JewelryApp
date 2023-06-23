using JewelryApp.Models.Dtos;

namespace JewelryApp.Client.Services;

public interface IPriceService
{
    public Task<PriceModel?> GetPriceModel();
}